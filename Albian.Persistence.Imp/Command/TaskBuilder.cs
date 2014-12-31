#region

using System;
using System.Collections.Generic;
using System.Reflection;
using Albian.Persistence.Context;
using Albian.Persistence.Imp.Cache;
using Albian.Persistence.Imp.Context;
using Albian.Persistence.Model;
using log4net;

#endregion

namespace Albian.Persistence.Imp.Command
{
    public class TaskBuilder : ITaskBuilder
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region ITaskBuilder Members

        public ITask BuildCreateTask<T>(T target)
            where T : IAlbianObject
        {
            ITask task = new Task();
            IFakeCommandBuilder fakeBuilder = new FakeCommandBuilder();
            IStorageContextBuilder storageContextBuilder = new StorageContextBuilder();
            task.Context = storageContextBuilder.GenerateStorageContexts(target,
                                                                         fakeBuilder.GenerateFakeCommandByRoutings,
                                                                         fakeBuilder.BuildCreateFakeCommandByRouting);
            foreach (KeyValuePair<string, IStorageContext> context in task.Context)
            {
                IStorageContext storageContext = context.Value;
                object oStorage = StorageCache.Get(context.Key);
                if (null == oStorage)
                {
                    if (null != Logger)
                        Logger.ErrorFormat("There is no {0} storage attribute in the storage cached.",
                                           storageContext.StorageName);
                    return null;
                }
                IStorageAttribute storage = (IStorageAttribute) oStorage;
                storageContext.Storage = storage;
            }
            return task;
        }

        public ITask BuildCreateTask<T>(IList<T> target)
            where T : IAlbianObject
        {
            ITask task = new Task();
            IFakeCommandBuilder fakeBuilder = new FakeCommandBuilder();
            IStorageContextBuilder storageContextBuilder = new StorageContextBuilder();
            foreach (T o in target)
            {
                IDictionary<string, IStorageContext> storageContexts = storageContextBuilder.GenerateStorageContexts(o,
                                                                                                                     fakeBuilder
                                                                                                                         .
                                                                                                                         GenerateFakeCommandByRoutings,
                                                                                                                     fakeBuilder
                                                                                                                         .
                                                                                                                         BuildCreateFakeCommandByRouting);
                if (null == storageContexts || 0 == storageContexts.Count)
                {
                    if (null != Logger)
                        Logger.Error("The storagecontexts is empty.");
                    throw new Exception("The storagecontexts is null.");
                }
                if (null == task.Context || 0 == task.Context.Count)
                {
                    task.Context = storageContexts;
                    continue;
                }
                foreach (KeyValuePair<string, IStorageContext> storageContext in storageContexts)
                {
                    if (task.Context.ContainsKey(storageContext.Key))
                    {
                        task.Context[storageContext.Key].FakeCommand = task.Context.ContainsKey(storageContext.Key)
                                                                           ? Utils.Concat(
                                                                                 task.Context[storageContext.Key].
                                                                                     FakeCommand,
                                                                                 storageContext.Value.FakeCommand)
                                                                           : storageContext.Value.FakeCommand;
                    }
                    else
                    {
                        task.Context.Add(storageContext);
                    }
                }
            }

            foreach (KeyValuePair<string, IStorageContext> context in task.Context)
            {
                IStorageContext storageContext = context.Value;
                object oStorage = StorageCache.Get(context.Key);
                if (null == oStorage)
                {
                    if (null != Logger)
                        Logger.ErrorFormat("There is no {0} storage attribute in the storage cached.",
                                           storageContext.StorageName);
                    return null;
                }
                IStorageAttribute storage = (IStorageAttribute) oStorage;
                storageContext.Storage = storage;
            }
            return task;
        }


        public ITask BuildModifyTask<T>(T target)
            where T : IAlbianObject
        {
            ITask task = new Task();
            IFakeCommandBuilder fakeBuilder = new FakeCommandBuilder();
            IStorageContextBuilder storageContextBuilder = new StorageContextBuilder();
            task.Context = storageContextBuilder.GenerateStorageContexts(target,
                                                                         fakeBuilder.GenerateFakeCommandByRoutings,
                                                                         fakeBuilder.BuildModifyFakeCommandByRouting);
            foreach (KeyValuePair<string, IStorageContext> context in task.Context)
            {
                IStorageContext storageContext = context.Value;
                object oStorage = StorageCache.Get(context.Key);
                if (null == oStorage)
                {
                    if (null != Logger)
                        Logger.ErrorFormat("There is no {0} storage attribute in the storage cached.",
                                           storageContext.StorageName);
                    return null;
                }
                IStorageAttribute storage = (IStorageAttribute) oStorage;
                storageContext.Storage = storage;
            }
            return task;
        }

        public ITask BuildModifyTask<T>(IList<T> target)
            where T : IAlbianObject
        {
            ITask task = new Task();
            IFakeCommandBuilder fakeBuilder = new FakeCommandBuilder();
            IStorageContextBuilder storageContextBuilder = new StorageContextBuilder();
            foreach (T o in target)
            {
                IDictionary<string, IStorageContext> storageContexts = storageContextBuilder.GenerateStorageContexts(o,
                                                                                                                     fakeBuilder
                                                                                                                         .
                                                                                                                         GenerateFakeCommandByRoutings,
                                                                                                                     fakeBuilder
                                                                                                                         .
                                                                                                                         BuildModifyFakeCommandByRouting);
                if (null == storageContexts || 0 == storageContexts.Count)
                {
                    if (null != Logger)
                        Logger.Error("The storagecontexts is empty.");
                    throw new Exception("The storagecontexts is null.");
                }
                if (null == task.Context || 0 == task.Context.Count)
                {
                    task.Context = storageContexts;
                    continue;
                }
                foreach (KeyValuePair<string, IStorageContext> storageContext in storageContexts)
                {
                    if (task.Context.ContainsKey(storageContext.Key))
                    {
                        task.Context[storageContext.Key].FakeCommand = task.Context.ContainsKey(storageContext.Key)
                                                                           ? Utils.Concat(
                                                                                 task.Context[storageContext.Key].
                                                                                     FakeCommand,
                                                                                 storageContext.Value.FakeCommand)
                                                                           : storageContext.Value.FakeCommand;
                    }
                    else
                    {
                        task.Context.Add(storageContext);
                    }
                }
            }

            foreach (KeyValuePair<string, IStorageContext> context in task.Context)
            {
                IStorageContext storageContext = context.Value;
                object oStorage = StorageCache.Get(context.Key);
                if (null == oStorage)
                {
                    if (null != Logger)
                        Logger.ErrorFormat("There is no {0} storage attribute in the storage cached.",
                                           storageContext.StorageName);
                    return null;
                }
                IStorageAttribute storage = (IStorageAttribute) oStorage;
                storageContext.Storage = storage;
            }
            return task;
        }

        public ITask BuildRemoveTask<T>(T target)
            where T : IAlbianObject
        {
            ITask task = new Task();
            IFakeCommandBuilder fakeBuilder = new FakeCommandBuilder();
            IStorageContextBuilder storageContextBuilder = new StorageContextBuilder();
            task.Context = storageContextBuilder.GenerateStorageContexts(target,
                                                                         fakeBuilder.GenerateFakeCommandByRoutings,
                                                                         fakeBuilder.BuildDeleteFakeCommandByRouting);
            foreach (KeyValuePair<string, IStorageContext> context in task.Context)
            {
                IStorageContext storageContext = context.Value;
                object oStorage = StorageCache.Get(context.Key);
                if (null == oStorage)
                {
                    if (null != Logger)
                        Logger.ErrorFormat("There is no {0} storage attribute in the storage cached.",
                                           storageContext.StorageName);
                    return null;
                }
                IStorageAttribute storage = (IStorageAttribute) oStorage;
                storageContext.Storage = storage;
            }
            return task;
        }

        public ITask BuildRemoveTask<T>(IList<T> target)
            where T : IAlbianObject
        {
            ITask task = new Task();
            IFakeCommandBuilder fakeBuilder = new FakeCommandBuilder();
            IStorageContextBuilder storageContextBuilder = new StorageContextBuilder();
            foreach (T o in target)
            {
                IDictionary<string, IStorageContext> storageContexts = storageContextBuilder.GenerateStorageContexts(o,
                                                                                                                     fakeBuilder
                                                                                                                         .
                                                                                                                         GenerateFakeCommandByRoutings,
                                                                                                                     fakeBuilder
                                                                                                                         .
                                                                                                                         BuildDeleteFakeCommandByRouting);
                if (null == storageContexts || 0 == storageContexts.Count)
                {
                    if (null != Logger)
                        Logger.Error("The storagecontexts is empty.");
                    throw new Exception("The storagecontexts is null.");
                }
                if (null == task.Context || 0 == task.Context.Count)
                {
                    task.Context = storageContexts;
                    continue;
                }
                foreach (KeyValuePair<string, IStorageContext> storageContext in storageContexts)
                {
                    if (task.Context.ContainsKey(storageContext.Key))
                    {
                        task.Context[storageContext.Key].FakeCommand = task.Context.ContainsKey(storageContext.Key)
                                                                           ? Utils.Concat(
                                                                                 task.Context[storageContext.Key].
                                                                                     FakeCommand,
                                                                                 storageContext.Value.FakeCommand)
                                                                           : storageContext.Value.FakeCommand;
                    }
                    else
                    {
                        task.Context.Add(storageContext);
                    }
                }
            }

            foreach (KeyValuePair<string, IStorageContext> context in task.Context)
            {
                IStorageContext storageContext = context.Value;
                object oStorage = StorageCache.Get(context.Key);
                if (null == oStorage)
                {
                    if (null != Logger)
                        Logger.ErrorFormat("There is no {0} storage attribute in the storage cached.",
                                           storageContext.StorageName);
                    return null;
                }
                IStorageAttribute storage = (IStorageAttribute) oStorage;
                storageContext.Storage = storage;
            }
            return task;
        }


        public ITask BuildSaveTask<T>(T target)
            where T : IAlbianObject
        {
            ITask task = new Task();
            IFakeCommandBuilder fakeBuilder = new FakeCommandBuilder();
            IStorageContextBuilder storageContextBuilder = new StorageContextBuilder();
            task.Context = storageContextBuilder.GenerateStorageContexts(target,
                                                                         fakeBuilder.GenerateFakeCommandByRoutings,
                                                                         fakeBuilder.BuildSaveFakeCommandByRouting);
            foreach (KeyValuePair<string, IStorageContext> context in task.Context)
            {
                IStorageContext storageContext = context.Value;
                object oStorage = StorageCache.Get(context.Key);
                if (null == oStorage)
                {
                    if (null != Logger)
                        Logger.ErrorFormat("There is no {0} storage attribute in the storage cached.",
                                           storageContext.StorageName);
                    return null;
                }
                IStorageAttribute storage = (IStorageAttribute) oStorage;
                storageContext.Storage = storage;
            }
            return task;
        }

        public ITask BuildSaveTask<T>(IList<T> target)
            where T : IAlbianObject
        {
            ITask task = new Task();
            IFakeCommandBuilder fakeBuilder = new FakeCommandBuilder();
            IStorageContextBuilder storageContextBuilder = new StorageContextBuilder();
            foreach (T o in target)
            {
                IDictionary<string, IStorageContext> storageContexts = storageContextBuilder.GenerateStorageContexts(o,
                                                                                                                     fakeBuilder
                                                                                                                         .
                                                                                                                         GenerateFakeCommandByRoutings,
                                                                                                                     fakeBuilder
                                                                                                                         .
                                                                                                                         BuildSaveFakeCommandByRouting);
                if (null == storageContexts || 0 == storageContexts.Count)
                {
                    if (null != Logger)
                        Logger.Error("The storagecontexts is empty.");
                    throw new Exception("The storagecontexts is null.");
                }
                if (null == task.Context || 0 == task.Context.Count)
                {
                    task.Context = storageContexts;
                    continue;
                }
                foreach (KeyValuePair<string, IStorageContext> storageContext in storageContexts)
                {
                    if (task.Context.ContainsKey(storageContext.Key))
                    {
                        task.Context[storageContext.Key].FakeCommand = task.Context.ContainsKey(storageContext.Key)
                                                                           ? Utils.Concat(
                                                                                 task.Context[storageContext.Key].
                                                                                     FakeCommand,
                                                                                 storageContext.Value.FakeCommand)
                                                                           : storageContext.Value.FakeCommand;
                    }
                    else
                    {
                        task.Context.Add(storageContext);
                    }
                }
            }

            foreach (KeyValuePair<string, IStorageContext> context in task.Context)
            {
                IStorageContext storageContext = context.Value;
                object oStorage = StorageCache.Get(context.Key);
                if (null == oStorage)
                {
                    if (null != Logger)
                        Logger.ErrorFormat("There is no {0} storage attribute in the storage cached.",
                                           storageContext.StorageName);
                    return null;
                }
                IStorageAttribute storage = (IStorageAttribute) oStorage;
                storageContext.Storage = storage;
            }
            return task;
        }

        public ITask BuildQueryTask<T>(string rountingName, int top, IFilterCondition[] where,
                                       IOrderByCondition[] orderby)
            where T : class, IAlbianObject, new()
        {
            ITask task = new Task();
            IFakeCommandBuilder fakeBuilder = new FakeCommandBuilder();
            IStorageContextBuilder storageContextBuilder = new StorageContextBuilder();
            IDictionary<string, IStorageContext> storageContexts =
                storageContextBuilder.GenerateStorageContexts<T>(rountingName, top, where, orderby);

            task.Context = storageContexts;
            foreach (KeyValuePair<string, IStorageContext> context in task.Context)
            {
                IStorageContext storageContext = context.Value;
                object oStorage = StorageCache.Get(context.Key);
                if (null == oStorage)
                {
                    if (null != Logger)
                        Logger.ErrorFormat("There is no {0} storage attribute in the storage cached.",
                                           storageContext.StorageName);
                    return null;
                }
                IStorageAttribute storage = (IStorageAttribute) oStorage;
                storageContext.Storage = storage;
            }
            return task;
        }

        #endregion
    }
}