#region

using System;
using System.Data;
using System.Data.OracleClient;
using System.Reflection;
using Albian.Persistence.ConnectionPool;
using log4net;

#endregion

namespace Albian.Persistence.Imp.ConnectionPool.Factory
{
    public class OracleConnectionFactory : IPoolableConnectionFactory<OracleConnection>
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region IPoolableConnectionFactory<OracleConnection> Members

        /// <summary>
        /// 创建对象
        /// </summary>
        public OracleConnection CreateObject()
        {
            OracleConnection obj = new OracleConnection();
            return obj;
        }

        /// <summary>
        /// 销毁对象.
        /// </summary>
        public void DestroyObject(OracleConnection obj)
        {
            if (ConnectionState.Closed != obj.State)
            {
                obj.Close();
            }
            if (obj is IDisposable)
            {
                ((IDisposable) obj).Dispose();
            }
        }

        /// <summary>
        /// 检查并确保对象的安全
        /// </summary>
        public bool ValidateObject(OracleConnection obj)
        {
            return null != obj;
        }

        /// <summary>
        /// 激活对象池中待用对象. 
        /// </summary>
        public void ActivateObject(OracleConnection obj, string connectionString)
        {
            try
            {
                if (ConnectionState.Open == obj.State) return;
                obj.ConnectionString = connectionString;
                if (ConnectionState.Open != obj.State)
                    obj.Open();
            }
            catch (Exception exc)
            {
                if (null != Logger) Logger.WarnFormat("连接池激活对象时发生异常,异常信息为:{0}", exc.Message);
            }
        }

        /// <summary>
        /// 卸载内存中正在使用的对象.
        /// </summary>
        public void PassivateObject(OracleConnection obj)
        {
            if (ConnectionState.Closed != obj.State) obj.Close();
        }

        #endregion
    }
}