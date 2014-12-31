#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using Albian.Persistence.ConnectionPool;
using log4net;

#endregion

namespace Albian.Persistence.Imp.ConnectionPool
{
    /// <summary>
    ///  对象池
    /// </summary>
    public class ConnectionPool<T> : IConnectionPool where T : IDbConnection, new()
    {
        private static readonly object locker = new object();
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IList<IDbConnection> _busy = new List<IDbConnection>();
        private readonly IPoolableConnectionFactory<T> _factory;
        private readonly int _maxSize = 30;
        private readonly int _minSize = 15;
        private bool _closed;
        private int _currentSize;
        private IList<IDbConnection> _free = new List<IDbConnection>();

        public ConnectionPool(IPoolableConnectionFactory<T> factory, int minSize, int maxSize)
        {
            if (null == factory)
            {
                if (null != Logger)
                    Logger.Error("创建对象池时发生异常，对象池化工厂不能为空");
                throw new ArgumentNullException("factory", "对象创建工厂不能为空！");
            }
            _factory = factory;
            _minSize = minSize;
            _maxSize = maxSize;
            InitItems(minSize);
            _currentSize = minSize;

            if (null != Logger)
                Logger.InfoFormat("对象池已经创建，对象池初始长度为：{0}，最大长度为{1}", _minSize, _maxSize);
        }

        #region IConnectionPool Members

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public IDbConnection GetObject(string connectionString)
        {
            return DoGetObject(connectionString);
        }

        /// <summary>
        /// 将使用完毕的对象返回到对象池.
        /// </summary>
        public void ReturnObject(IDbConnection target)
        {
            DoReturnObject((T) target);
        }

        /// <summary>
        /// 关闭对象池并释放池中所有的资源
        /// </summary>
        public void Close()
        {
            DoClose();
        }

        /// <summary>
        /// 得到当前对象池中正在使用的对象数. 
        /// </summary>
        public int NumActive
        {
            get { return _busy.Count; }
        }

        /// <summary>
        /// 得到当前对象池中可用的对象数
        /// </summary>
        public int NumIdle
        {
            get { return _free.Count; }
        }

        /// <summary>
        /// 强行创建一个对象
        /// </summary>
        /// <returns></returns>
        public IDbConnection RescueObject(string connectionString)
        {
            return DoRescueObject(connectionString);
        }

        #endregion

        protected void InitItems(int initialInstances)
        {
            if (initialInstances <= 0)
            {
                if (null != Logger)
                    Logger.Error("实例化对象池项时发生异常：对象池长度不能为空");
                throw new ArgumentException("对象池长度不能为空！", "initialInstances");
            }
            for (int i = 0; i < initialInstances; ++i)
            {
                _free.Add(_factory.CreateObject());
            }
        }

        protected IDbConnection DoGetObject(string connectionString)
        {
            bool isLock = false;
            try
            {
                if (_closed)
                {
                    if (null != Logger)
                        Logger.Warn("从对象池中获取对象时发生异常：对象池已经关闭，无法取得对象，对象池自行创建一个短连接对象。");
                    return RescueObject(connectionString);
                }
                if (!Monitor.TryEnter(locker, 1000)) //默认等1秒
                {
                    if (null != Logger)
                        Logger.Warn("对象池锁阻塞，无法取得对象，对象池自行创建一个短连接对象。");
                    if (_currentSize < _maxSize)
                    {
                        IDbConnection target = RescueObject(connectionString);
                        _currentSize++;
                        return target;
                    }
                    else
                    {
                        Logger.Warn("对象池锁阻塞，无法取得对象，并且池内size已经达到最大限制，无法自行创建一个短连接对象。");
                        throw new Exception("The poolsize is overflow.");
                    }
                }
                isLock = true;
                while (_free.Count > 0)
                {
                    int i = _free.Count - 1;
                    IDbConnection o = _free[i];
                    _free.RemoveAt(i);
                    _factory.ActivateObject((T) o, connectionString);
                    if (!_factory.ValidateObject((T) o)) continue;

                    _busy.Add(o);
                    if (null != Logger)
                        Logger.InfoFormat("连接池状态：现在空闲对象长度为:{0},忙碌对象长度为{1}.", NumIdle, NumActive);
                    return (T) o;
                }

                if (null != Logger)
                    Logger.InfoFormat("从对象池中获取对象时发生异常：对象池中没有可用对象!现在空闲对象长度为:{0},忙碌对象长度为{1}.", NumIdle, NumActive);
                return RescueObject(connectionString);
            }
            catch (Exception exc)
            {
                if (null != Logger)
                    Logger.ErrorFormat("从对象池中获取对象时发生异常：{0}.", exc.Message);
                return RescueObject(connectionString);
            }
            finally
            {
                try
                {
                    if (isLock) Monitor.Exit(locker);
                }
                catch (Exception exc)
                {
                    if (isLock)
                    {
                        if (null != Logger)
                            Logger.ErrorFormat("对象池中释放对象锁时发生异常：{0}.", exc.Message);
                    }
                    else
                    {
                        if (null != Logger)
                            Logger.ErrorFormat("在未获取锁的对象上释放锁时发生异常：{0}.", exc.Message);
                    }
                }
            }
        }

        protected bool DoReturnObject(T target)
        {
            if (_closed)
            {
                _factory.DestroyObject(target);
                if (null != Logger) Logger.Info("连接池已经关闭，放回对象被释放！");
                return true;
            }
            lock (locker)
            {
                if (_busy.Contains(target))
                {
                    if (null != Logger) Logger.Info("连接对象使用完毕，准备放回连接池！");
                    _busy.Remove(target);
                    _factory.PassivateObject(target);
                    _free.Add(target);
                    if (null != Logger) Logger.Info("连接对象使用完毕，放回连接池！");
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 关闭对象池
        /// </summary>
        private void DoClose()
        {
            _free = new List<IDbConnection>();
            _closed = true;
        }

        protected T DoRescueObject(string connectionString)
        {
            T obj = _factory.CreateObject();
            _factory.ActivateObject(obj, connectionString);
            return obj;
        }
    }
}