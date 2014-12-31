#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Albian.Kernel.Service;
using Albian.Persistence.Context;
using Albian.Persistence.Imp.Cache;
using Albian.Persistence.Imp.Command;
using Albian.Persistence.Imp.ConnectionPool;
using Albian.Persistence.Imp.Parser.Impl;
using Albian.Persistence.Model;
using log4net;

#endregion

namespace Albian.Persistence.Imp.Query
{
    public class QueryCluster : IQueryCluster
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region IQueryCluster Members

        public T QueryObject<T>(ITask task)
            where T : class, IAlbianObject, new()
        {
            IList<T> targets = QueryObjects<T>(task);
            if (null == targets || 1 != targets.Count)
            {
                throw new PersistenceException("There are more objects in the databse result.");
            }
            return targets[0];
        }

        public IList<T> QueryObjects<T>(ITask task)
            where T : class, IAlbianObject, new()
        {
            Hashtable reader;
            IDictionary<string, IMemberAttribute> members;
            IList<T> objects = new List<T>();
            IDataReader dr = Execute(task);
            try
            {
                PropertyInfo[] properties = AfterExecute<T>(dr, out reader, out members);
                while (dr.Read())
                {
                    T target = AlbianObjectCreater<T>(properties, dr, reader, members);
                    objects.Add(target);
                }
                return objects;
            }
            finally
            {
                dr.Close();
                dr.Dispose();
                foreach (KeyValuePair<string, IStorageContext> kv in task.Context)
                {
                    if(null != kv.Value.Connection)
                    {
                        if (ConnectionState.Closed != kv.Value.Connection.State)
                        {
                            kv.Value.Connection.Close();
                        }
                        if (kv.Value.Storage.Pooling)
                        {
                            DbConnectionPoolManager.RetutnConnection(kv.Value.StorageName, kv.Value.Connection);
                        }
                        else
                        {
                            //kv.Value.StorageName,
                            kv.Value.Connection.Dispose();
                            //conn = null;
                        }
                    }
                }
            }
        }

        public T QueryObject<T>(IDbCommand cmd)
            where T : class, IAlbianObject, new()
        {
            IList<T> targets = QueryObjects<T>(cmd);
            if (null == targets || 1 != targets.Count)
            {
                throw new PersistenceException("There are more objects in the databse result.");
            }
            return targets[0];
        }

        public IList<T> QueryObjects<T>(IDbCommand cmd)
            where T : class, IAlbianObject, new()
        {
            Hashtable reader;
            IDictionary<string, IMemberAttribute> members;
            IList<T> objects = new List<T>();
            IDataReader dr = Execute(cmd);
            try
            {
                PropertyInfo[] properties = AfterExecute<T>(dr, out reader, out members);
                while (dr.Read())
                {
                    T target = AlbianObjectCreater<T>(properties, dr, reader, members);
                    objects.Add(target);
                }
                return objects;
            }
            finally
            {
                dr.Close();
                dr.Dispose();
            }
        }

        #endregion

        protected virtual IDataReader Execute(IDbCommand cmd)
        {
            if (null == cmd)
            {
                throw new ArgumentNullException("cmd");
            }
            try
            {
                if (ConnectionState.Open != cmd.Connection.State)
                    cmd.Connection.Open();
                IDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return dr;
            }
            catch (Exception exc)
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
                throw exc;
            }
        }

        protected virtual IDataReader Execute(ITask task)
        {
            if (null == task)
            {
                throw new ArgumentNullException("task");
            }
            if (1 != task.Context.Count) //only one
            {
                throw new PersistenceException("The query task is error.");
            }
            IStorageContext storageContext = PreExecute(task);

            try
            {
                if (ConnectionState.Open != storageContext.Connection.State)
                    storageContext.Connection.Open();

                IFakeCommandAttribute fc = storageContext.FakeCommand[0]; //only one
                IDbCommand cmd = storageContext.Connection.CreateCommand();
                cmd.CommandText = fc.CommandText;
                cmd.CommandType = CommandType.Text;
                foreach (DbParameter para in fc.Paras)
                {
                    cmd.Parameters.Add(para);
                }
                IDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return dr;
            }
            catch (Exception exc)
            {
                if (storageContext.Connection.State != ConnectionState.Closed)
                    storageContext.Connection.Close();
                throw exc;
            }
        }

        protected IStorageContext PreExecute(ITask task)
        {
            IStorageContext[] storageContexts = new IStorageContext[task.Context.Values.Count];
            task.Context.Values.CopyTo(storageContexts, 0);
            //task.Context.
            //IStorageContext storageContext = task.Context.
            IStorageContext storageContext = storageContexts[0];

            string sConnection = StorageParser.BuildConnectionString(storageContext.Storage);
            storageContext.Connection =
                storageContext.Storage.Pooling
                    ?
                        DbConnectionPoolManager.GetConnection(storageContext.StorageName, sConnection)
                    :
                        DatabaseFactory.GetDbConnection(storageContext.Storage.DatabaseStyle, sConnection);
            return storageContext;
        }

        protected PropertyInfo[] AfterExecute<T>(IDataReader dr, out Hashtable reader,
                                                 out IDictionary<string, IMemberAttribute> members)
        {
            Type type = typeof (T);
            string fullName = AssemblyManager.GetFullTypeName(type);
            object oProperties = PropertyCache.Get(fullName);
            PropertyInfo[] properties;
            if (null == oProperties)
            {
                if (null != Logger)
                    Logger.Warn("Get the object property info from cache is null.Reflection now and add to cache.");
                properties = type.GetProperties();
                PropertyCache.InsertOrUpdate(fullName, properties);
            }
            properties = (PropertyInfo[]) oProperties;
            object oAttribute = ObjectCache.Get(fullName);
            if (null == oAttribute)
            {
                if (null != Logger)
                    Logger.ErrorFormat("The {0} object attribute is null in the object cache.", fullName);
                throw new Exception("The object attribute is null");
            }
            IObjectAttribute objectAttribute = (IObjectAttribute) oAttribute;
            members = objectAttribute.MemberAttributes;

            reader = new Hashtable();
            for (int i = 0; i < dr.FieldCount; i++)
            {
                reader[dr.GetName(i)] = i;
            }
            return properties;
        }

        protected T AlbianObjectCreater<T>(PropertyInfo[] properties, IDataReader dr, Hashtable reader,
                                           IDictionary<string, IMemberAttribute> members)
            where T :class, IAlbianObject,new()
        {
            T target = AlbianObjectFactory.CreateInstance<T>();
            foreach (PropertyInfo property in properties)
            {
                IMemberAttribute member = members[property.Name];
                if (!member.IsSave)
                {
                    if (property.Name == "IsNew") //define by interface)
                        property.SetValue(target, false, null); //load from databse
                    continue;
                }
                object value = dr.GetValue(int.Parse(reader[member.FieldName].ToString()));
                if (null == value || DBNull.Value == value)
                {
                    //property.SetValue(target, null, null);
                    continue;
                }
                property.SetValue(target, value, null);
            }
            return target;
        }
    }
}