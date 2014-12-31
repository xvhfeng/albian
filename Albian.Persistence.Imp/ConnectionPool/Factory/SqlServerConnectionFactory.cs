#region

using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Albian.Persistence.ConnectionPool;
using log4net;

#endregion

namespace Albian.Persistence.Imp.ConnectionPool.Factory
{
    /// <summary>
    /// 连接池创建工厂
    /// </summary>
    public class SqlServerConnectionFactory : IPoolableConnectionFactory<SqlConnection>
        //where T : IDbConnection, new()
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region IPoolableConnectionFactory<SqlConnection> Members

        /// <summary>
        /// 创建对象
        /// </summary>
        public SqlConnection CreateObject()
        {
            SqlConnection obj = new SqlConnection();
            return obj;
        }

        /// <summary>
        /// 销毁对象.
        /// </summary>
        public void DestroyObject(SqlConnection obj)
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
        public bool ValidateObject(SqlConnection obj)
        {
            return null != obj;
        }

        /// <summary>
        /// 激活对象池中待用对象. 
        /// </summary>
        public void ActivateObject(SqlConnection obj, string connectionString)
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
        public void PassivateObject(SqlConnection obj)
        {
            if (ConnectionState.Closed != obj.State) obj.Close();
        }

        #endregion
    }
}