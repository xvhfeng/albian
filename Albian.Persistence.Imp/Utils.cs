#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Albian.Kernel.Service;
using Albian.Persistence.Enum;
using Albian.Persistence.Model;

#endregion

namespace Albian.Persistence.Imp
{
    public class Utils
    {
        public static IList<T> Concat<T>(IList<T> target, IList<T> source)
        {
            foreach (T o in source)
            {
                target.Add(o);
            }
            return target;
        }

        public static string GetTableFullName(IRoutingAttribute routing, IAlbianObject target)
        {
            HashAlbianObjectHandler<IAlbianObject> handler = HashAlbianObjectManager.GetHandler(routing.Name, AssemblyManager.GetFullTypeName(target));
            string tableName = null == handler
                                   ? routing.TableName
                                   : String.Format("{0}{1}", routing.TableName, handler(target));
            return "dbo" == routing.Owner || String.IsNullOrEmpty(routing.Owner)
                       ? tableName
                       : String.Format("[{0}].[{1}]", routing.Owner, tableName);
        }

        public static bool IsNullableType(Type type)
        {
            return (type.IsGenericType && type.
                                              GetGenericTypeDefinition().Equals
                                              (typeof (Nullable<>)));
        }

        public static string GetRelationalOperators(RelationalOperators opt)
        {
            switch (opt)
            {
                case RelationalOperators.And:
                    {
                        return "AND";
                    }
                case RelationalOperators.OR:
                    {
                        return "OR";
                    }
                default:
                    {
                        return "AND";
                    }
            }
        }

        public static string GetLogicalOperation(LogicalOperation opt)
        {
            switch (opt)
            {
                case LogicalOperation.Equal:
                    {
                        return "=";
                    }
                case LogicalOperation.Greater:
                    {
                        return ">";
                    }
                case LogicalOperation.GreaterOrEqual:
                    {
                        return ">=";
                    }
                case LogicalOperation.Is:
                    {
                        return "IS";
                    }
                case LogicalOperation.Less:
                    {
                        return "<";
                    }
                case LogicalOperation.LessOrEqual:
                    {
                        return "<=";
                    }
                case LogicalOperation.NotEqual:
                    {
                        return "<>";
                    }
                default:
                    {
                        return "=";
                    }
            }
        }

        public static string GetCacheKey<T>(string routingName, int top, IFilterCondition[] where,
                                            IOrderByCondition[] orderby)
            where T : IAlbianObject
        {
            StringBuilder sbKey = new StringBuilder();
            sbKey.Append(AssemblyManager.GetFullTypeName(typeof (T)));
            sbKey.Append(routingName);
            if (0 != top)
                sbKey.Append(top);
            if (null != where)
            {
                foreach (IFilterCondition filter in where)
                {
                    sbKey.AppendFormat("{0}{1}{2}{3}", filter.Relational, filter.PropertyName.ToLower(), filter.Logical,
                                       DBNull.Value == filter.Value ? "NULL" : filter.Value);
                }
            }
            if (null != orderby)
            {
                foreach (IOrderByCondition sort in orderby)
                {
                    sbKey.Append(sort.PropertyName.ToLower()).Append(sort.SortStyle);
                }
            }
            return sbKey.ToString();
        }

        public static string GetCacheKey<T>(IDbCommand cmd)
            where T : IAlbianObject
        {
            StringBuilder sbKey = new StringBuilder();
            if (null != cmd.Connection)
            {
                sbKey.Append(cmd.Connection.ConnectionString);
            }
            if (string.IsNullOrEmpty(cmd.CommandText))
            {
                sbKey.Append(cmd.CommandText);
            }
            if (null != cmd.Parameters)
            {
                foreach (IDataParameter para in cmd.Parameters)
                {
                    sbKey.Append(para.ParameterName).Append(DBNull.Value == para.Value ? "NULL" : para.Value);
                }
            }

            return sbKey.ToString();
        }

        public static string GetCacheKey<T>(string idValue,string typeFullName)
            where T : IAlbianObject
        {
            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat("{0}${1}", typeFullName, idValue);
            return sbKey.ToString();
        }
    }
}