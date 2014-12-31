#region

using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Reflection;
using Albian.Persistence.Enum;
using log4net;
using MySql.Data.MySqlClient;

#endregion

namespace Albian.Persistence.Imp.Command
{
    public class DatabaseFactory
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string GetParameterName(DatabaseStyle databaseStyle, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (null != name)
                    Logger.Warn("The database parameter is empty.");
                throw new ArgumentNullException("name");
            }
            switch (databaseStyle)
            {
                case DatabaseStyle.Oracle:
                    {
                        return string.Format(":{0}", name);
                    }
                case DatabaseStyle.MySql:
                case DatabaseStyle.SqlServer:
                default:
                    {
                        return string.Format("@{0}", name);
                    }
            }
        }

        public static DbParameter GetDbParameter(DatabaseStyle databaseStyle, string name, DbType dbType, object value,
                                                 int size)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (null != name)
                    Logger.Warn("The database parameter is empty.");
                throw new ArgumentNullException("name");
            }
            switch (databaseStyle)
            {
                case DatabaseStyle.MySql:
                    {
                        DbParameter para = new MySqlParameter
                                               {
                                                   ParameterName = name,
                                                   Value = value
                                               };
                        if (DbType.AnsiString == dbType || DbType.AnsiStringFixedLength == dbType ||
                            DbType.String == dbType || DbType.StringFixedLength == dbType)
                        {
                            para.Size = size;
                        }

                        return para;
                    }
                case DatabaseStyle.Oracle:
                    {
                        DbParameter para = new OracleParameter
                                               {
                                                   ParameterName = name,
                                                   Value = value
                                               };
                        if (DbType.AnsiString == dbType || DbType.AnsiStringFixedLength == dbType ||
                            DbType.String == dbType || DbType.StringFixedLength == dbType)
                        {
                            para.Size = size;
                        }

                        return para;
                    }
                case DatabaseStyle.SqlServer:
                default:
                    {
                        DbParameter para = new SqlParameter
                                               {
                                                   ParameterName = name,
                                                   Value = value
                                               };
                        if (DbType.AnsiString == dbType || DbType.AnsiStringFixedLength == dbType ||
                            DbType.String == dbType || DbType.StringFixedLength == dbType)
                        {
                            para.Size = size;
                        }

                        return para;
                    }
            }
        }

        public static IDbConnection GetDbConnection(DatabaseStyle dbStyle, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                if (null != connectionString)
                    Logger.Warn("The database connectionString is empty.");
                throw new ArgumentNullException("connectionString");
            }
            try
            {
                switch (dbStyle)
                {
                    case DatabaseStyle.MySql:
                        {
                            return new MySqlConnection(connectionString);
                        }
                    case DatabaseStyle.Oracle:
                        {
                            return new OracleConnection(connectionString);
                        }
                    case DatabaseStyle.SqlServer:
                    default:
                        {
                            return new SqlConnection(connectionString);
                        }
                }
            }
            catch (Exception exc)
            {
                if (null != Logger)
                    Logger.ErrorFormat("Create the connection is error.Message:{0}.", exc.Message);
                throw exc;
            }
        }
    }
}