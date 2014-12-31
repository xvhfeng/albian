#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Albian.Persistence.Context;
using Albian.Persistence.Enum;
using Albian.Persistence.Imp.Cache;
using Albian.Persistence.Imp.Command;
using Albian.Persistence.Imp.Model;
using Albian.Persistence.Imp.Parser.Impl;
using Albian.Persistence.Imp.Query;
using Albian.Persistence.Imp.TransactionCluster;
using Albian.Persistence.Model;
using log4net;

#endregion

namespace Albian.Persistence.Imp
{
    public static class PersistenceService
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static bool Create<T>(T albianObject)
            where T : class, IAlbianObject
        {
            if (null == albianObject)
            {
                throw new ArgumentNullException("albianObject");
            }
            TaskBuilder builder = new TaskBuilder();
            ITask task = builder.BuildCreateTask(albianObject);
            ITransactionClusterScope tran = new TransactionClusterScope();
            bool isSuccess = tran.Execute(task);
            if (!isSuccess) return isSuccess;
            ResultCache.CachingObject(albianObject);
            return isSuccess;
        }

        public static bool Create<T>(IList<T> albianObjects)
            where T : class, IAlbianObject
        {
            if (null == albianObjects)
            {
                throw new ArgumentNullException("albianObjects");
            }
            if (0 == albianObjects.Count)
            {
                throw new ArgumentException("albianObject count is 0.");
            }
            TaskBuilder builder = new TaskBuilder();
            ITask task = builder.BuildCreateTask(albianObjects);
            ITransactionClusterScope tran = new TransactionClusterScope();
            bool isSuccess = tran.Execute(task);
            if (!isSuccess) return isSuccess;
            ResultCache.CachingObjects(albianObjects);
            return isSuccess;
        }

        public static bool Modify<T>(T albianObject)
            where T : class, IAlbianObject
        {
            if (null == albianObject)
            {
                throw new ArgumentNullException("albianObject");
            }

            TaskBuilder builder = new TaskBuilder();
            ITask task = builder.BuildModifyTask(albianObject);
            ITransactionClusterScope tran = new TransactionClusterScope();
            bool isSuccess = tran.Execute(task);
            if (!isSuccess) return isSuccess;
            ResultCache.CachingObject(albianObject);
            return isSuccess;
        }

        public static bool Modify<T>(IList<T> albianObjects)
            where T : class, IAlbianObject
        {
            if (null == albianObjects)
            {
                throw new ArgumentNullException("albianObjects");
            }
            if (0 == albianObjects.Count)
            {
                throw new ArgumentException("albianObject count is 0.");
            }
            TaskBuilder builder = new TaskBuilder();
            ITask task = builder.BuildModifyTask(albianObjects);
            ITransactionClusterScope tran = new TransactionClusterScope();
            bool isSuccess = tran.Execute(task);
            if (!isSuccess) return isSuccess;
            ResultCache.CachingObjects(albianObjects);
            return isSuccess;
        }

        public static bool Remove<T>(T albianObject)
            where T : class, IAlbianObject
        {
            if (null == albianObject)
            {
                throw new ArgumentNullException("albianObject");
            }
            TaskBuilder builder = new TaskBuilder();
            ITask task = builder.BuildRemoveTask(albianObject);
            ITransactionClusterScope tran = new TransactionClusterScope();
            return tran.Execute(task);
        }

        public static bool Remove<T>(IList<T> albianObjects)
            where T : class, IAlbianObject
        {
            if (null == albianObjects)
            {
                throw new ArgumentNullException("albianObjects");
            }
            if (0 == albianObjects.Count)
            {
                throw new ArgumentException("albianObject count is 0.");
            }
            TaskBuilder builder = new TaskBuilder();
            ITask task = builder.BuildRemoveTask(albianObjects);
            ITransactionClusterScope tran = new TransactionClusterScope();
            return tran.Execute(task);
        }

        public static bool Save<T>(T albianObject)
            where T : class, IAlbianObject
        {
            if (null == albianObject)
            {
                throw new ArgumentNullException("albianObject");
            }

            TaskBuilder builder = new TaskBuilder();
            ITask task = builder.BuildSaveTask(albianObject);
            ITransactionClusterScope tran = new TransactionClusterScope();
            bool isSuccess = tran.Execute(task);
            if (!isSuccess) return isSuccess;
            ResultCache.CachingObject(albianObject);
            return isSuccess;
        }

        public static bool Save<T>(IList<T> albianObjects)
            where T : class, IAlbianObject
        {
            if (null == albianObjects)
            {
                throw new ArgumentNullException("albianObjects");
            }
            if (0 == albianObjects.Count)
            {
                throw new ArgumentException("albianObject count is 0.");
            }
            ITaskBuilder builder = new TaskBuilder();
            ITask task = builder.BuildSaveTask(albianObjects);
            ITransactionClusterScope tran = new TransactionClusterScope();
            bool isSuccess = tran.Execute(task);
            if (!isSuccess) return isSuccess;
            ResultCache.CachingObjects(albianObjects);
            return isSuccess;
        }

        public static T FindObject<T>(string routingName, IFilterCondition[] where)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            return DoFindObject<T>(routingName, where);
        }

        public static T FindObject<T>(string value)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            return DoFindObject<T>(PersistenceParser.DefaultRoutingName, new IFilterCondition[]
                                                                             {
                                                                                 new FilterCondition
                                                                                     {
                                                                                         Logical =
                                                                                             LogicalOperation.Equal,
                                                                                         PropertyName = "Id",
                                                                                         Relational =
                                                                                             RelationalOperators.And,
                                                                                         Value = value,
                                                                                     }
                                                                             });
        }

        public static T FindObject<T>(string routingName, string value)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            return DoFindObject<T>(routingName, new IFilterCondition[]
                                                    {
                                                        new FilterCondition
                                                            {
                                                                Logical = LogicalOperation.Equal,
                                                                PropertyName = "Id",
                                                                Relational = RelationalOperators.And,
                                                                Value = value,
                                                            }
                                                    });
        }

        public static T FindObject<T>(IFilterCondition[] where)
            where T : class, IAlbianObject, new()
        {
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            return DoFindObject<T>(PersistenceParser.DefaultRoutingName, where);
        }

        public static T FindObject<T>(IDbCommand cmd)
            where T : class, IAlbianObject, new()
        {
            if (null == cmd)
            {
                throw new ArgumentNullException("cmd");
            }
            return DoFindObject<T>(cmd);
        }

        public static IList<T> FindObjects<T>(int top, IFilterCondition[] where, IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            if (0 == top)
            {
                throw new ArgumentException("The 'top' is 0.");
            }
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            if (null == orderby)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == orderby.Length)
            {
                throw new ArgumentException("orderby length is 0.");
            }

            return DoFindObjects<T>(PersistenceParser.DefaultRoutingName, top, where, orderby);
        }

        public static IList<T> FindObjects<T>(IFilterCondition[] where)
            where T : class, IAlbianObject, new()
        {
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            return DoFindObjects<T>(PersistenceParser.DefaultRoutingName, 0, where, null);
        }

        public static IList<T> FindObjects<T>(IFilterCondition[] where, IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            if (null == orderby)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == orderby.Length)
            {
                throw new ArgumentException("orderby length is 0.");
            }

            return DoFindObjects<T>(PersistenceParser.DefaultRoutingName, 0, where, orderby);
        }

        public static IList<T> FindObjects<T>(IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            if (null == orderby)
            {
                throw new ArgumentNullException("orderby");
            }
            if (0 == orderby.Length)
            {
                throw new ArgumentException("orderby length is 0.");
            }

            return DoFindObjects<T>(PersistenceParser.DefaultRoutingName, 0, null, orderby);
        }

        public static IList<T> FindObjects<T>(string routingName, IFilterCondition[] where, IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            if (null == orderby)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == orderby.Length)
            {
                throw new ArgumentException("orderby length is 0.");
            }

            return DoFindObjects<T>(routingName, 0, where, orderby);
        }

        public static IList<T> FindObjects<T>(string routingName, IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (null == orderby)
            {
                throw new ArgumentNullException("orderby");
            }
            if (0 == orderby.Length)
            {
                throw new ArgumentException("orderby length is 0.");
            }
            return DoFindObjects<T>(routingName, 0, null, orderby);
        }

        public static IList<T> FindObjects<T>(string routingName, IFilterCondition[] where)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }

            return DoFindObjects<T>(routingName, 0, where, null);
        }

        public static IList<T> FindObjects<T>(string routingName, int top, IFilterCondition[] where,
                                              IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (0 == top)
            {
                throw new ArgumentException("The 'top' is 0.");
            }
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            if (null == orderby)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == orderby.Length)
            {
                throw new ArgumentException("orderby length is 0.");
            }
            return DoFindObjects<T>(routingName, top, where, orderby);
        }

        public static IList<T> FindObjects<T>(string routingName, int top, IFilterCondition[] where)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (0 == top)
            {
                throw new ArgumentException("The 'top' is 0.");
            }
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            return DoFindObjects<T>(routingName, top, where, null);
        }

        public static IList<T> FindObjects<T>(IDbCommand cmd)
            where T : class, IAlbianObject, new()
        {
            if (null == cmd)
            {
                throw new ArgumentNullException("cmd");
            }
            return DoFindObjects<T>(cmd);
        }

        public static T LoadObject<T>(string routingName, IFilterCondition[] where)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            return DoLoadObject<T>(routingName, where);
        }

        public static T LoadObject<T>(string value)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            return DoLoadObject<T>(PersistenceParser.DefaultRoutingName, new IFilterCondition[]
                                                                             {
                                                                                 new FilterCondition
                                                                                     {
                                                                                         Logical =
                                                                                             LogicalOperation.Equal,
                                                                                         PropertyName = "Id",
                                                                                         Relational =
                                                                                             RelationalOperators.And,
                                                                                         Value = value,
                                                                                     }
                                                                             });
        }


        public static T LoadObject<T>(string routingName, string value)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            return DoLoadObject<T>(routingName, new IFilterCondition[]
                                                    {
                                                        new FilterCondition
                                                            {
                                                                Logical = LogicalOperation.Equal,
                                                                PropertyName = "Id",
                                                                Relational = RelationalOperators.And,
                                                                Value = value,
                                                            }
                                                    });
        }


        public static T LoadObject<T>(IFilterCondition[] where)
            where T : class, IAlbianObject, new()
        {
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            return DoLoadObject<T>(PersistenceParser.DefaultRoutingName, where);
        }

        public static T LoadObject<T>(IDbCommand cmd)
            where T : class, IAlbianObject, new()
        {
            if (null == cmd)
            {
                throw new ArgumentNullException("cmd");
            }
            return DoLoadObject<T>(cmd);
        }

        public static IList<T> LoadObjects<T>(int top, IFilterCondition[] where, IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            if (0 == top)
            {
                throw new ArgumentException("The 'top' is 0.");
            }
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            if (null == orderby)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == orderby.Length)
            {
                throw new ArgumentException("orderby length is 0.");
            }
            return DoLoadObjects<T>(PersistenceParser.DefaultRoutingName, top, where, orderby);
        }

        public static IList<T> LoadObjects<T>(IFilterCondition[] where)
            where T : class, IAlbianObject, new()
        {
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            return DoLoadObjects<T>(PersistenceParser.DefaultRoutingName, 0, where, null);
        }

        public static IList<T> LoadObjects<T>(IFilterCondition[] where, IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            if (null == orderby)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == orderby.Length)
            {
                throw new ArgumentException("orderby length is 0.");
            }
            return DoLoadObjects<T>(PersistenceParser.DefaultRoutingName, 0, where, orderby);
        }

        public static IList<T> LoadObjects<T>(IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            if (null == orderby)
            {
                throw new ArgumentNullException("orderby");
            }
            if (0 == orderby.Length)
            {
                throw new ArgumentException("orderby length is 0.");
            }

            return DoLoadObjects<T>(PersistenceParser.DefaultRoutingName, 0, null, orderby);
        }

        public static IList<T> LoadObjects<T>(string routingName, IFilterCondition[] where, IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            if (null == orderby)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == orderby.Length)
            {
                throw new ArgumentException("orderby length is 0.");
            }
            return DoLoadObjects<T>(routingName, 0, where, orderby);
        }

        public static IList<T> LoadObjects<T>(string routingName, IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (null == orderby)
            {
                throw new ArgumentNullException("orderby");
            }
            if (0 == orderby.Length)
            {
                throw new ArgumentException("orderby length is 0.");
            }
            return DoLoadObjects<T>(routingName, 0, null, orderby);
        }

        public static IList<T> LoadObjects<T>(string routingName, IFilterCondition[] where)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            return DoLoadObjects<T>(routingName, 0, where, null);
        }

        public static IList<T> LoadObjects<T>(string routingName, int top, IFilterCondition[] where,
                                              IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (0 == top)
            {
                throw new ArgumentException("The 'top' is 0.");
            }
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            if (null == orderby)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == orderby.Length)
            {
                throw new ArgumentException("orderby length is 0.");
            }
            return DoLoadObjects<T>(routingName, top, where, orderby);
        }

        public static IList<T> LoadObjects<T>(string routingName, int top, IFilterCondition[] where)
            where T : class, IAlbianObject, new()
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            if (0 == top)
            {
                throw new ArgumentException("The 'top' is 0.");
            }
            if (null == where)
            {
                throw new ArgumentNullException("where");
            }
            if (0 == where.Length)
            {
                throw new ArgumentException("where length is 0.");
            }
            return DoLoadObjects<T>(routingName, top, where, null);
        }

        public static IList<T> LoadObjects<T>(IDbCommand cmd)
            where T : class, IAlbianObject, new()
        {
            if (null == cmd)
            {
                throw new ArgumentNullException("cmd");
            }
            return DoLoadObjects<T>(cmd);
        }


        private static T DoFindObject<T>(string routingName, IFilterCondition[] where)
            where T : class, IAlbianObject, new()
        {
            try
            {
                T target = ResultCache.GetCachingObject<T>(routingName, where);
                if (null != target) return target;
                target = DoLoadObject<T>(routingName, where);
                //ResultCache.CachingObject(routingName, where, target);
                return target;
            }
            catch (Exception exc)
            {
                if (null != Logger)
                    Logger.ErrorFormat("Find Object is error.info:{0}.", exc.Message);
                throw;
            }
        }

        private static T DoFindObject<T>(IDbCommand cmd)
            where T : class, IAlbianObject,new()
        {
            try
            {
                T target = ResultCache.GetCachingObject<T>(cmd);
                if (null != target) return target;
                target = DoLoadObject<T>(cmd);
                //ResultCache.CachingObject(cmd, target);
                return target;
            }
            catch (Exception exc)
            {
                if (null != Logger)
                    Logger.ErrorFormat("Find Object is error..info:{0}.", exc.Message);
                throw;
            }
        }

        private static IList<T> DoFindObjects<T>(string routingName, int top, IFilterCondition[] where,
                                                 IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            try
            {
                IList<T> target = ResultCache.GetCachingObjects<T>(routingName, top, where, orderby);
                if (null != target) return target;
                target = DoLoadObjects<T>(routingName, top, where, orderby);
                //ResultCache.CachingObjects(routingName, top, where, orderby, target);
                return target;
            }
            catch (Exception exc)
            {
                if (null != Logger)
                    Logger.ErrorFormat("Find Object is error..info:{0}.", exc.Message);
                throw exc;
            }
        }

        private static IList<T> DoFindObjects<T>(IDbCommand cmd)
            where T : class, IAlbianObject, new()
        {
            try
            {
                IList<T> target = ResultCache.GetCachingObjects<T>(cmd);
                if (null != target) return target;
                target = DoLoadObjects<T>(cmd);
                //ResultCache.CachingObjects(cmd, target);
                return target;
            }
            catch (Exception exc)
            {
                if (null != Logger)
                    Logger.ErrorFormat("Find Object is error..info:{0}.", exc.Message);
                throw exc;
            }
        }

        private static T DoLoadObject<T>(string routingName, IFilterCondition[] where)
            where T : class, IAlbianObject, new()
        {
            try
            {
                ITaskBuilder taskBuilder = new TaskBuilder();
                ITask task = taskBuilder.BuildQueryTask<T>(routingName, 0, where, null);
                IQueryCluster query = new QueryCluster();
                T target = query.QueryObject<T>(task);
                ResultCache.CachingObject(routingName, where, target);
                return target;
            }
            catch (Exception exc)
            {
                if (null != Logger)
                    Logger.ErrorFormat("load Object is error..info:{0}.", exc.Message);
                throw;
            }
        }

        private static T DoLoadObject<T>(IDbCommand cmd)
            where T : class, IAlbianObject, new()
        {
            try
            {
                IQueryCluster query = new QueryCluster();
                T target = query.QueryObject<T>(cmd);
                ResultCache.CachingObject(cmd, target);
                return target;
            }
            catch (Exception exc)
            {
                if (null != Logger)
                    Logger.ErrorFormat("load Object is error..info:{0}.", exc.Message);
                throw;
            }
        }

        private static IList<T> DoLoadObjects<T>(string routingName, int top, IFilterCondition[] where,
                                                 IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            try
            {
                ITaskBuilder taskBuilder = new TaskBuilder();
                ITask task = taskBuilder.BuildQueryTask<T>(routingName, top, where, orderby);
                IQueryCluster query = new QueryCluster();
                IList<T> targets = query.QueryObjects<T>(task);
                ResultCache.CachingObjects(routingName, top, where, orderby, targets);
                return targets;
            }
            catch (Exception exc)
            {
                if (null != Logger)
                    Logger.ErrorFormat("Find Object is error..info:{0}.", exc.Message);
                throw;
            }
        }

        private static IList<T> DoLoadObjects<T>(IDbCommand cmd)
            where T : class, IAlbianObject, new()
        {
            try
            {
                IQueryCluster query = new QueryCluster();
                IList<T> targets = query.QueryObjects<T>(cmd);
                ResultCache.CachingObjects(cmd, targets);
                return targets;
            }
            catch (Exception exc)
            {
                if (null != Logger)
                    Logger.ErrorFormat("Find Object is error..info:{0}.", exc.Message);
                throw;
            }
        }
    }
}