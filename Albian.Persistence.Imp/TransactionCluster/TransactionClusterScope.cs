#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Albian.Persistence.Context;
using Albian.Persistence.Imp.Cache;
using Albian.Persistence.Imp.Command;
using Albian.Persistence.Imp.ConnectionPool;
using Albian.Persistence.Imp.Notify;
using Albian.Persistence.Imp.Parser.Impl;
using Albian.Persistence.Model;
using log4net;
using Albian.Kernel.Service.Impl;

#endregion

namespace Albian.Persistence.Imp.TransactionCluster
{
    public class TransactionClusterScope : ITransactionClusterScope
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private TransactionClusterState _state = TransactionClusterState.NoStarted;

        #region ITransactionClusterScope Members

        /// <summary>
        /// 当前事务集群的状态
        /// </summary>
        public virtual TransactionClusterState State
        {
            get { return _state; }
        }

        /// <summary>
        /// 自动执行事务，并且提交或者回滚
        /// </summary>
        /// <param name="task"></param>
        public virtual bool Execute(ITask task)
        {
            bool isSuccess = false;
            IDictionary<string, IStorageContext> contexts = task.Context;
            _state = TransactionClusterState.NoStarted;
            try
            {
                _state = TransactionClusterState.Opening;

                PreLoadExecute(contexts);

                _state = TransactionClusterState.OpenedAndRuning;

                ExecuteHandler(contexts);

                _state = TransactionClusterState.Runned;

                Executed(contexts);

                _state = TransactionClusterState.Commited;
                isSuccess = true;
            }
            catch (Exception exc)
            {
                isSuccess = false;
                if (null != Logger)
                {
                    Logger.ErrorFormat("Execute the cluster transaction scope is error.info:{0}", exc.Message);
                }
                _state = TransactionClusterState.Rollbacking;
                ExceptionHandler(contexts);
                _state = TransactionClusterState.Rollbacked;
            }
            finally
            {
                UnLoadExecute(contexts);
            }
            return isSuccess;
        }

        #endregion

        protected virtual void UnLoadExecute(IDictionary<string, IStorageContext> storageContexts)
        {
            foreach (KeyValuePair<string, IStorageContext> context in storageContexts)
            {
                IStorageContext storageContext = context.Value;
                try
                {
                    if (null != context.Value.Command)
                    {
                        foreach (IDbCommand cmd in context.Value.Command)
                        {
                            cmd.Parameters.Clear();
                            cmd.Dispose();
                        }
                    }
                    if (storageContext.Storage.Transactional && null != storageContext.Transaction)
                    {
                        storageContext.Transaction.Dispose();
                    }
                    storageContext.Transaction = null;
                    storageContext.FakeCommand = null;

                    if (null != storageContext.Connection &&
                        ConnectionState.Closed != storageContext.Connection.State)
                    {
                        storageContext.Connection.Close();

                        if (storageContext.Storage.Pooling)
                        {
                            DbConnectionPoolManager.RetutnConnection(storageContext.StorageName,
                                                                     storageContext.Connection);
                        }
                        else
                        {
                            storageContext.Connection.Dispose();
                        }
                        storageContext.Connection = null;
                    }
                }
                catch
                {
                    if (null != Logger)
                        Logger.Warn("Clear the database resources is error.but must close the all connections");
                }
            }
        }

        protected virtual void Executed(IDictionary<string, IStorageContext> storageContexts)
        {
            foreach (KeyValuePair<string, IStorageContext> context in storageContexts)
            {
                if (context.Value.Storage.Transactional)
                {
                    context.Value.Transaction.Commit();
                }
                _state = TransactionClusterState.Commiting;
            }
        }

        protected virtual void ExceptionHandler(IDictionary<string, IStorageContext> storageContexts)
        {
            foreach (KeyValuePair<string, IStorageContext> context in storageContexts)
            {
                if (context.Value.Storage.Transactional)
                {
                    try
                    {
                        context.Value.Transaction.Rollback();
                    }
                    catch (Exception exc)
                    {
                        if (null != Logger)
                            Logger.ErrorFormat("Rollback is error.Message:{0}.",exc.Message);
                    }
                }
            }
        }

        protected virtual void ExecuteHandler(IDictionary<string, IStorageContext> storageContexts)
        {
            foreach (KeyValuePair<string, IStorageContext> context in storageContexts)
            {
                foreach (IDbCommand cmd in context.Value.Command)
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected virtual void PreLoadExecute(IDictionary<string, IStorageContext> storageContexts)
        {
            foreach (KeyValuePair<string, IStorageContext> context in storageContexts)
            {
                IStorageContext storageContext = context.Value;
                string sConnection = StorageParser.BuildConnectionString(storageContext.Storage);
                storageContext.Connection =
                    storageContext.Storage.Pooling
                        ?
                            DbConnectionPoolManager.GetConnection(storageContext.StorageName, sConnection)
                        :
                            DatabaseFactory.GetDbConnection(storageContext.Storage.DatabaseStyle, sConnection);

                try
                {
                    if (ConnectionState.Open != storageContext.Connection.State)
                        storageContext.Connection.Open();
                }
                catch (Exception exc)
                {
                    IStorageAttribute storageAttr = (IStorageAttribute)StorageCache.Get(storageContext.StorageName);
                    storageAttr.IsHealth = false;
                    UnhealthyStorage.Add(storageAttr.Name);
                    if (null != Logger)
                        Logger.WarnFormat("Storage:{0} can not open.Set the health is false and it not used until the health set true.",storageAttr.Name);
                    IConnectionNotify notify = AlbianServiceRouter.GetService<IConnectionNotify>();
                    if (null != notify)
                    {
                        Logger.Info("send message when open database is error.");
                        string msg = string.Format("Server:{0},Database:{1},Exception Message:{2}.",storageContext.Storage.Server,storageContext.Storage.Database,exc.Message);
                        notify.SendMessage(msg);
                    }
                    throw exc;
                }
                if (storageContext.Storage.Transactional)
                {
                    storageContext.Transaction =
                        storageContext.Connection.BeginTransaction(IsolationLevel.ReadUncommitted);
                }
                foreach (IFakeCommandAttribute fc in storageContext.FakeCommand)
                {
                    IDbCommand cmd = storageContext.Connection.CreateCommand();
                    cmd.CommandText = fc.CommandText;
                    cmd.CommandType = CommandType.Text;
                    if (storageContext.Storage.Transactional)
                    {
                        cmd.Transaction = storageContext.Transaction;
                    }
                    foreach (DbParameter para in fc.Paras)
                    {
                        cmd.Parameters.Add(para);
                    }
                    storageContext.Command.Add(cmd);
                }
            }
        }
    }
}