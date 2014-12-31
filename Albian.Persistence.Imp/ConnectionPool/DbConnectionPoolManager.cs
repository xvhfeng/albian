#region

using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Reflection;
using Albian.Persistence.ConnectionPool;
using Albian.Persistence.Enum;
using Albian.Persistence.Imp.ConnectionPool.Factory;
using log4net;
using MySql.Data.MySqlClient;

#endregion

namespace Albian.Persistence.Imp.ConnectionPool
{
    public sealed class DbConnectionPoolManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 创建连接池.
        /// </summary>
        /// <remarks>
        /// isTrackerPool,isStoragePool不能同时为true或者false
        /// </remarks>
        public static void CreatePool(string storageName, DatabaseStyle dbStyle, int minSize, int maxSize)
        {
            switch (dbStyle)
            {
                case DatabaseStyle.MySql:
                    {
                        IConnectionPool pool =
                            new ConnectionPool<MySqlConnection>(new MySqlConnectionFactory(), minSize, maxSize);
                        ConnectionPoolCached.InsertOrUpdate(storageName, pool);
                        break;
                    }
                case DatabaseStyle.Oracle:
                    {
                        IConnectionPool pool =
                            new ConnectionPool<OracleConnection>(new OracleConnectionFactory(), minSize, maxSize);
                        ConnectionPoolCached.InsertOrUpdate(storageName, pool);
                        break;
                    }
                case DatabaseStyle.SqlServer:
                default:
                    {
                        IConnectionPool pool =
                            new ConnectionPool<SqlConnection>(new SqlServerConnectionFactory(), minSize, maxSize);
                        ConnectionPoolCached.InsertOrUpdate(storageName, pool);
                        break;
                    }
            }
        }

        /// <summary>
        /// 得到指定的连接池.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public static IConnectionPool GetPool(string stroageName)
        {
            object obj = ConnectionPoolCached.Get(stroageName);
            if (null == obj)
            {
                Logger.Warn("the pool is null.");
                return null;
                ;
            }
            return (IConnectionPool) obj;
        }

        public static IDbConnection GetConnection(string storageName, string connectionString)
        {
            IConnectionPool pool = GetPool(storageName);
            if (null == pool)
            {
                if (null != Logger)
                {
                    Logger.ErrorFormat("The {0} pool is empty.", storageName);
                }
                return null;
                //throw new Exception(string.Format("The {0} pool is empty.", storageContext.StorageName));
            }
            IDbConnection connection = pool.GetObject(connectionString);
            if (null == connection)
            {
                if (null != Logger)
                {
                    Logger.ErrorFormat("The connection is empty from the {0} pool.", storageName);
                }
                //throw new Exception(string.Format("The connection is empty from the {0} pool.", storageContext.StorageName));
            }
            return connection;
        }

        public static void RetutnConnection(string storageName, IDbConnection connection)
        {
            if (null == connection)
            {
                if (null != Logger)
                {
                    Logger.ErrorFormat("The connection is empty from the {0} pool.", storageName);
                }
                throw new ArgumentNullException("connection");
            }
            IConnectionPool pool = GetPool(storageName);
            if (null == pool)
            {
                if (null != Logger)
                {
                    Logger.ErrorFormat("The {0} pool is empty.", storageName);
                }
                return;
            }
            pool.ReturnObject(connection);
        }
    }
}