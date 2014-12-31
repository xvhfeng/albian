#region

using System;
using System.Collections.Generic;
using System.Reflection;
using Albian.Kernel.Service;
using Albian.Persistence.Context;
using Albian.Persistence.Imp.Cache;
using Albian.Persistence.Imp.Context;
using Albian.Persistence.Model;
using log4net;

#endregion

namespace Albian.Persistence.Imp.Command
{
    public class StorageContextBuilder : IStorageContextBuilder
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region IStorageContextBuilder Members

        public IDictionary<string, IStorageContext> GenerateStorageContexts<T>(T target,
                                                                               BuildFakeCommandByRoutingsHandler<T>
                                                                                   buildFakeCommandByRoutingsHandler,
                                                                               BuildFakeCommandByRoutingHandler<T>
                                                                                   buildFakeCommandByRoutingHandler)
            where T : IAlbianObject
        {
            if (null == target)
            {
                throw new ArgumentNullException("target");
            }

            Type type = target.GetType();
            string fullName = AssemblyManager.GetFullTypeName(target);
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
            IDictionary<string, IStorageContext> storageContexts = buildFakeCommandByRoutingsHandler(target, properties,
                                                                                                     objectAttribute,
                                                                                                     buildFakeCommandByRoutingHandler);

            if (0 == storageContexts.Count) //no the storage context
            {
                if (null != Logger)
                    Logger.Warn("There is no storage contexts of the object.");
                return null;
            }
            return storageContexts;
        }

        public IDictionary<string, IStorageContext> GenerateStorageContexts<T>(string rountingName, int top,
                                                                               IFilterCondition[] where,
                                                                               IOrderByCondition[] orderby)
            where T : class, IAlbianObject,new()
        {
            IDictionary<string, IStorageContext> storageContexts = new Dictionary<string, IStorageContext>();
            IFakeCommandBuilder fakeBuilder = new FakeCommandBuilder();
            IFakeCommandAttribute fakeCommandAttrribute = fakeBuilder.GenerateQuery<T>(rountingName, top, where, orderby);

            if (null == fakeCommandAttrribute) //the PermissionMode is not enough
            {
                if (null != Logger)
                    Logger.WarnFormat("The permission is not enough in the {0} routing.", rountingName);
                throw new PersistenceException(string.Format("The permission is not enough in the {0} routing.",
                                                             rountingName));
            }

            IStorageContext storageContext = new StorageContext
                                                 {
                                                     FakeCommand = new List<IFakeCommandAttribute>(),
                                                     StorageName = fakeCommandAttrribute.StorageName,
                                                 };
            storageContext.FakeCommand.Add(fakeCommandAttrribute);
            storageContexts.Add(fakeCommandAttrribute.StorageName, storageContext);
            return storageContexts;
        }

        #endregion
    }
}