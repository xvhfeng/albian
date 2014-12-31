using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using log4net;


namespace Albian.Foundation.ThreadPool
{
    public class ThreadPool : IThreadPool
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int _size;
        private readonly bool _isFlowExecutionContext;
        private readonly Queue<WorkItem> _queue = new Queue<WorkItem>();
        private Thread[] _threadPool;
        private bool _isClose;
        private int _semaphore;

        //默认每个cpu开25个线程，根据经验每个cpu开25个线程资源，性能比较平衡
        public ThreadPool()
            : this(Environment.ProcessorCount * 25, true)
        {

        }

        public ThreadPool(int size)
            : this(size, true)
        {
        }

        public ThreadPool(bool isFlowExecutionContext)
            : this(Environment.ProcessorCount * 25, isFlowExecutionContext)
        {

        }

        public ThreadPool(int size, bool isFlowExecutionContext)
        {
            if (size <= 0)
            {
                if (null != Logger)
                    Logger.ErrorFormat("线程池初始化size为:{0},不正确", size);
                throw new ArgumentOutOfRangeException(string.Format("线程池初始化size为:{0},不正确", size));
            }

            _size = size;
            _isFlowExecutionContext = isFlowExecutionContext;

            // 检查并且设置上下文执行权限
            if (!isFlowExecutionContext)
                new SecurityPermission(SecurityPermissionFlag.Infrastructure).Demand();
            if (null != Logger)
                Logger.InfoFormat("初始化线程池.Size为：{0},强制执行权限为：{1}", size, isFlowExecutionContext);
        }

        public void QueueUserWorkItem(WaitCallback work, object obj)
        {
            var item = new WorkItem(work, obj);

            if (_isFlowExecutionContext)
                item._context = ExecutionContext.Capture();

            Start();//补救措施

            lock (_queue)
            {
                _queue.Enqueue(item);
                if (_semaphore > 0)
                    Monitor.Pulse(_queue);
            }
        }

        public void Dispose()
        {
            _isClose = true;
            lock (_queue)
            {
                Monitor.PulseAll(_queue);
            }

            for (int i = 0; i < _threadPool.Length; i++)
                _threadPool[i].Join();
        }

        public void QueueUserWorkItem(WaitCallback work)
        {
            QueueUserWorkItem(work, null);
        }

        public void Start()
        {
            if (_threadPool == null)
            {
                if (null != Logger)
                    Logger.Info("开始初始化线程池.");
                lock (_queue)
                {
                    if (_threadPool == null)
                    {
                        _threadPool = new Thread[_size];
                        for (int i = 0; i < _threadPool.Length; i++)
                        {
                            _threadPool[i] = new Thread(CallbackProxy);
                            _threadPool[i].IsBackground = true;
                            _threadPool[i].Start();
                        }
                    }
                }
                if (null != Logger)
                    Logger.Info("初始化线程池成功.");
            }
        }

        private void CallbackProxy()
        {
            while (true)
            {
                WorkItem wi = default(WorkItem);

                lock (_queue)
                {
                    if (_isClose)
                        return;

                    while (_queue.Count == 0)
                    {
                        _semaphore++;
                        try
                        {
                            Monitor.Wait(_queue);
                        }
                        finally
                        {
                            _semaphore--;
                        }

                        if (_isClose)
                            return;
                    }

                    wi = _queue.Dequeue();
                }

                try
                {
                    wi.Invoke();
                }
                catch (Exception exc)//这里是否有更好的办法，调用proxy function时需要处理异常信息
                {
                    if (null != Logger)
                        Logger.Warn("执行方法时发生异常.", exc);
                    return;
                }
            }
        }
    }
}