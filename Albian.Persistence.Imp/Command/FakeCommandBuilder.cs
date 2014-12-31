#region

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;
using Albian.Kernel.Service;
using Albian.Persistence.Context;
using Albian.Persistence.Enum;
using Albian.Persistence.Imp.Cache;
using Albian.Persistence.Imp.Context;
using Albian.Persistence.Imp.Parser.Impl;
using Albian.Persistence.Model;
using Albian.Persistence.Model.Impl;
using log4net;

#endregion

namespace Albian.Persistence.Imp.Command
{
    public class FakeCommandBuilder : IFakeCommandBuilder
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region IFakeCommandBuilder Members

        public IDictionary<string, IStorageContext> GenerateFakeCommandByRoutings<T>(T target, PropertyInfo[] properties,
                                                                                     IObjectAttribute objectAttribute,
                                                                                     BuildFakeCommandByRoutingHandler<T>
                                                                                         buildFakeCommandByRoutingHandler)
            where T : IAlbianObject
        {
            if (null == properties || 0 == properties.Length)
            {
                throw new ArgumentNullException("properties");
            }
            if (null == objectAttribute)
            {
                throw new ArgumentNullException("objectAttribute");
            }
            if (null == objectAttribute.RoutingAttributes || null == objectAttribute.RoutingAttributes.Values ||
                0 == objectAttribute.RoutingAttributes.Values.Count)
            {
                if (null != Logger)
                    Logger.Error("The routing attributes or routings is null");
                throw new Exception("The routing attributes or routing is null");
            }

            IDictionary<string, IStorageContext> storageContexts = new Dictionary<string, IStorageContext>();
            foreach (var routing in objectAttribute.RoutingAttributes.Values)
            {
                IFakeCommandAttribute fakeCommandAttrribute = buildFakeCommandByRoutingHandler(PermissionMode.W, target,
                                                                                               routing, objectAttribute,
                                                                                               properties);
                if (null == fakeCommandAttrribute) //the PermissionMode is not enough
                {
                    if (null != Logger)
                        Logger.WarnFormat("The permission is not enough in the {0} routing.", routing.Name);
                    continue;
                }
                if (storageContexts.ContainsKey(fakeCommandAttrribute.StorageName))
                {
                    storageContexts[fakeCommandAttrribute.StorageName].FakeCommand.Add(fakeCommandAttrribute);
                }
                else
                {
                    IStorageContext storageContext = new StorageContext
                                                         {
                                                             FakeCommand = new List<IFakeCommandAttribute>(),
                                                             StorageName = fakeCommandAttrribute.StorageName,
                                                         };
                    storageContext.FakeCommand.Add(fakeCommandAttrribute);
                    storageContexts.Add(fakeCommandAttrribute.StorageName, storageContext);
                }
            }
            return storageContexts;
        }

        public IFakeCommandAttribute BuildCreateFakeCommandByRouting<T>(PermissionMode permission, T target,
                                                                        IRoutingAttribute routing,
                                                                        IObjectAttribute objectAttribute,
                                                                        PropertyInfo[] properties)
            where T : IAlbianObject
        {
            if (null == routing)
            {
                throw new ArgumentNullException("routing");
            }
            if (null == properties || 0 == properties.Length)
            {
                throw new ArgumentNullException("properties");
            }
            if (null == objectAttribute)
            {
                throw new ArgumentNullException("objectAttribute");
            }
            if (0 == (permission & routing.Permission))
            {
                if (null != Logger)
                    Logger.WarnFormat("The routing permission {0} is no enough.", permission);
                return null;
            }

           

            //create the connection string
            IStorageAttribute storageAttr = (IStorageAttribute) StorageCache.Get(routing.StorageName);
           
            if (null == storageAttr)
            {
                if (null != Logger)
                    Logger.WarnFormat(
                        "No {0} rounting mapping storage attribute in the sotrage cache.Use default storage.",
                        routing.Name);
                storageAttr = (IStorageAttribute) StorageCache.Get(StorageParser.DefaultStorageName);
            }

            if (!storageAttr.IsHealth)
            {
                if(null != Logger)
                    Logger.WarnFormat("Routing:{0},Storage:{1} is not health.", routing.Name, storageAttr.Name);
                return null;
            }

            var sbInsert = new StringBuilder();
            var sbCols = new StringBuilder();
            var sbValues = new StringBuilder();

            IList<DbParameter> paras = new List<DbParameter>();

            //create the hash table name
            string tableFullName = Utils.GetTableFullName(routing, target);

            //build the command text
            IDictionary<string, IMemberAttribute> members = objectAttribute.MemberAttributes;
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(target, null);
                if (null == value)
                {
                    continue;
                }
                IMemberAttribute member = members[property.Name];
                if (!member.IsSave)
                {
                    continue;
                }
                sbCols.AppendFormat("{0},", member.FieldName);
                string paraName = DatabaseFactory.GetParameterName(storageAttr.DatabaseStyle, member.FieldName);
                sbValues.AppendFormat("{0},", paraName);
                paras.Add(DatabaseFactory.GetDbParameter(storageAttr.DatabaseStyle, paraName, member.DBType, value,
                                                         member.Length));
            }
            int colsLen = sbCols.Length;
            if (0 < colsLen)
            {
                sbCols.Remove(colsLen - 1, 1);
            }
            int valLen = sbValues.Length;
            if (0 < valLen)
            {
                sbValues.Remove(valLen - 1, 1);
            }
            sbInsert.AppendFormat("INSERT INTO {0} ({1}) VALUES({2}) ", tableFullName, sbCols, sbValues);
            IFakeCommandAttribute fakeCmd = new FakeCommandAttribute
                                                {
                                                    CommandText = sbInsert.ToString(),
                                                    Paras = ((List<DbParameter>) paras).ToArray(),
                                                    StorageName = routing.StorageName
                                                };
            return fakeCmd;
        }

        public IFakeCommandAttribute BuildModifyFakeCommandByRouting<T>(PermissionMode permission, T target,
                                                                        IRoutingAttribute routing,
                                                                        IObjectAttribute objectAttribute,
                                                                        PropertyInfo[] properties)
            where T : IAlbianObject
        {
            if (null == routing)
            {
                throw new ArgumentNullException("routing");
            }
            if (null == properties || 0 == properties.Length)
            {
                throw new ArgumentNullException("properties");
            }
            if (null == objectAttribute)
            {
                throw new ArgumentNullException("objectAttribute");
            }
            if (0 == (permission & routing.Permission))
            {
                if (null != Logger)
                    Logger.WarnFormat("The routing permission {0} is no enough.", permission);
                return null;
            }

            IList<DbParameter> paras = new List<DbParameter>();

            //create the connection string
            IStorageAttribute storageAttr = (IStorageAttribute) StorageCache.Get(routing.StorageName);
            if (null == storageAttr)
            {
                if (null != Logger)
                    Logger.WarnFormat(
                        "No {0} rounting mapping storage attribute in the sotrage cache.Use default storage.",
                        routing.Name);
                storageAttr = (IStorageAttribute) StorageCache.Get(StorageParser.DefaultStorageName);
            }

            if (!storageAttr.IsHealth)
            {
                if (null != Logger)
                    Logger.WarnFormat("Routing:{0},Storage:{1} is not health.", routing.Name, storageAttr.Name);
                return null;
            }

            //create the hash table name
            string tableFullName = Utils.GetTableFullName(routing, target);

            //build the command text
            IDictionary<string, IMemberAttribute> members = objectAttribute.MemberAttributes;
            IDictionary<string, IMemberAttribute> pks = objectAttribute.PrimaryKeys;
            if (null == pks || 0 == pks.Count)
            {
                throw new Exception("Can not Update the Database,the pks is null or empty.");
            }

            StringBuilder sbPKs = new StringBuilder();
            StringBuilder sbCols = new StringBuilder();
            StringBuilder sbUpdate = new StringBuilder();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(target, null);
                if (null == value)
                {
                    continue;
                }
                IMemberAttribute member = members[property.Name];
                if (!member.IsSave)
                {
                    continue;
                }

                string paraName = DatabaseFactory.GetParameterName(storageAttr.DatabaseStyle, member.FieldName);

                if (member.PrimaryKey)
                {
                    sbPKs.AppendFormat("AND {0}={1} ", member.FieldName, paraName);
                    paras.Add(DatabaseFactory.GetDbParameter(storageAttr.DatabaseStyle, paraName, member.DBType, value,
                                                             member.Length));
                    continue;
                }

                sbCols.AppendFormat("{0}={1},", member.FieldName, paraName);
                paras.Add(DatabaseFactory.GetDbParameter(storageAttr.DatabaseStyle, paraName, member.DBType, value,
                                                         member.Length));
            }
            int colsLen = sbCols.Length;
            if (0 < colsLen)
            {
                sbCols.Remove(colsLen - 1, 1);
            }

            sbUpdate.AppendFormat("UPDATE {0} SET {1} WHERE 1=1 {2} ", tableFullName, sbCols, sbPKs);

            IFakeCommandAttribute fakeCmd = new FakeCommandAttribute
                                                {
                                                    CommandText = sbUpdate.ToString(),
                                                    Paras = ((List<DbParameter>) paras).ToArray(),
                                                    StorageName = routing.StorageName
                                                };
            return fakeCmd;
        }

        public IFakeCommandAttribute BuildDeleteFakeCommandByRouting<T>(PermissionMode permission, T target,
                                                                        IRoutingAttribute routing,
                                                                        IObjectAttribute objectAttribute,
                                                                        PropertyInfo[] properties)
            where T : IAlbianObject
        {
            if (null == routing)
            {
                throw new ArgumentNullException("routing");
            }
            if (null == properties || 0 == properties.Length)
            {
                throw new ArgumentNullException("properties");
            }
            if (null == objectAttribute)
            {
                throw new ArgumentNullException("objectAttribute");
            }
            if (0 == (permission & routing.Permission))
            {
                if (null != Logger)
                    Logger.WarnFormat("The routing permission {0} is no enough.", permission);
                return null;
            }

            StringBuilder sbDelete = new StringBuilder();
            StringBuilder sbPKs = new StringBuilder();

            IList<DbParameter> paras = new List<DbParameter>();

            //create the connection string
            IStorageAttribute storageAttr = (IStorageAttribute) StorageCache.Get(routing.StorageName);
            if (null == storageAttr)
            {
                if (null != Logger)
                    Logger.WarnFormat(
                        "No {0} rounting mapping storage attribute in the sotrage cache.Use default storage.",
                        routing.Name);
                storageAttr = (IStorageAttribute) StorageCache.Get(StorageParser.DefaultStorageName);
            }

            if (!storageAttr.IsHealth)
            {
                if (null != Logger)
                    Logger.WarnFormat("Routing:{0},Storage:{1} is not health.", routing.Name, storageAttr.Name);
                return null;
            }


            //create the hash table name
            string tableFullName = Utils.GetTableFullName(routing, target);

            //build the command text
            IDictionary<string, IMemberAttribute> members = objectAttribute.MemberAttributes;
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(target, null);
                if (null == value)
                {
                    continue;
                }
                IMemberAttribute member = members[property.Name];
                if (!member.PrimaryKey)
                {
                    continue;
                }

                string paraName = DatabaseFactory.GetParameterName(storageAttr.DatabaseStyle, member.FieldName);

                sbPKs.AppendFormat(" AND {0}={1}", member.FieldName, paraName);
                paras.Add(DatabaseFactory.GetDbParameter(storageAttr.DatabaseStyle, paraName, member.DBType, value,
                                                         member.Length));
            }

            sbDelete.AppendFormat("DELETE FROM {0} WHERE 1=1 {1} ", tableFullName, sbPKs);
            IFakeCommandAttribute fakeCmd = new FakeCommandAttribute
                                                {
                                                    CommandText = sbDelete.ToString(),
                                                    Paras = ((List<DbParameter>) paras).ToArray(),
                                                    StorageName = routing.StorageName
                                                };
            return fakeCmd;
        }

        public IFakeCommandAttribute BuildSaveFakeCommandByRouting<T>(PermissionMode permission, T target,
                                                                      IRoutingAttribute routing,
                                                                      IObjectAttribute objectAttribute,
                                                                      PropertyInfo[] properties)
            where T : IAlbianObject
        {
            if (null == routing)
            {
                throw new ArgumentNullException("routing");
            }
            if (null == properties || 0 == properties.Length)
            {
                throw new ArgumentNullException("properties");
            }
            if (null == objectAttribute)
            {
                throw new ArgumentNullException("objectAttribute");
            }
            if (0 == (permission & routing.Permission))
            {
                if (null != Logger)
                    Logger.WarnFormat("The routing permission {0} is no enough.", permission);
                return null;
            }

            return target.IsNew
                       ?
                           BuildCreateFakeCommandByRouting(permission, target, routing, objectAttribute, properties)
                       :
                           BuildModifyFakeCommandByRouting(permission, target, routing, objectAttribute, properties);
        }

        public IFakeCommandAttribute GenerateQuery<T>(string rountingName, int top, IFilterCondition[] where,
                                                      IOrderByCondition[] orderby)
            where T :class, IAlbianObject,new()
        {
            Type type = typeof (T);
            string fullName = AssemblyManager.GetFullTypeName(type);
            object oProperties = PropertyCache.Get(fullName);
            PropertyInfo[] properties;
            if (null == oProperties)
            {
                if (null != Logger)
                    Logger.Error("Get the object property info from cache is null.Reflection now and add to cache.");
                throw new PersistenceException("object property is null in the cache.");
            }
            properties = (PropertyInfo[]) oProperties;
            object oAttribute = ObjectCache.Get(fullName);
            if (null == oAttribute)
            {
                if (null != Logger)
                    Logger.ErrorFormat("The {0} object attribute is null in the object cache.", fullName);
                throw new Exception("The object attribute is null");
            }
            StringBuilder sbSelect = new StringBuilder();
            StringBuilder sbCols = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbOrderBy = new StringBuilder();
            IObjectAttribute objectAttribute = (IObjectAttribute) oAttribute;
            IRoutingAttribute routing;

            if (!objectAttribute.RoutingAttributes.TryGetValue(rountingName, out routing))
            {
                if (null != Logger)
                    Logger.WarnFormat("There is not routing of the {} object.Albian use the default routing tempate.",
                                      rountingName);
                routing = objectAttribute.RountingTemplate;
            }

            if (0 == (PermissionMode.R & routing.Permission))
            {
                if (null != Logger)
                    Logger.WarnFormat("The routing permission {0} is no enough.", routing.Permission);
                return null;
            }

            IStorageAttribute storageAttr = (IStorageAttribute) StorageCache.Get(routing.StorageName);
            if (null == storageAttr)
            {
                if (null != Logger)
                    Logger.WarnFormat(
                        "No {0} rounting mapping storage attribute in the sotrage cache.Use default storage.",
                        routing.Name);
                storageAttr = (IStorageAttribute) StorageCache.Get(StorageParser.DefaultStorageName);
            }

            if (!storageAttr.IsHealth)
            {
                if (null != Logger)
                    Logger.WarnFormat("Routing:{0},Storage:{1} is not health.", routing.Name, storageAttr.Name);
                return null;
            }

            IDictionary<string, IMemberAttribute> members = objectAttribute.MemberAttributes;
            T target = AlbianObjectFactory.CreateInstance<T>();
            foreach (PropertyInfo property in properties)
            {
                IMemberAttribute member = members[property.Name];
                if (!member.IsSave) continue;
                sbCols.AppendFormat("{0},", member.FieldName);

                if (null != where)
                {
                    foreach (IFilterCondition condition in where) //have better algorithm??
                    {
                        if (condition.PropertyName == property.Name)
                        {
                            property.SetValue(target, condition.Value, null); //Construct the splite object
                            break;
                        }
                    }
                }
                if (null != orderby)
                {
                    foreach (IOrderByCondition order in orderby)
                    {
                        if (order.PropertyName == property.Name)
                        {
                            sbOrderBy.AppendFormat("{0} {1},", member.FieldName,
                                                   System.Enum.GetName(typeof (SortStyle), order.SortStyle));
                            break;
                        }
                    }
                }
            }
            if (0 != sbOrderBy.Length)
            {
                sbOrderBy.Remove(sbOrderBy.Length - 1, 1);
            }
            if (0 != sbCols.Length)
            {
                sbCols.Remove(sbCols.Length - 1, 1);
            }
            IList<DbParameter> paras = new List<DbParameter>();

            if (null != where && 0 != where.Length)
            {
                foreach (IFilterCondition condition in where)
                {
                    IMemberAttribute member = members[condition.PropertyName];
                    if (!member.IsSave) continue;
                    sbWhere.AppendFormat(" {0} {1} {2} {3} ", Utils.GetRelationalOperators(condition.Relational),
                                         member.FieldName, Utils.GetLogicalOperation(condition.Logical),
                                         DatabaseFactory.GetParameterName(storageAttr.DatabaseStyle, member.FieldName));
                    paras.Add(DatabaseFactory.GetDbParameter(storageAttr.DatabaseStyle, member.FieldName, member.DBType,
                                                             condition.Value, member.Length));
                }
            }
            string tableFullName = Utils.GetTableFullName(routing, target);
            switch (storageAttr.DatabaseStyle)
            {
                case DatabaseStyle.MySql:
                    {
                        sbSelect.AppendFormat("SELECT {0} FROM {1} WHERE 1=1 {2} {3} {4}",
                                  sbCols, tableFullName, sbWhere,
                                  0 == sbOrderBy.Length ? string.Empty : string.Format("ORDER BY {0}", sbOrderBy),
                                  0 == top ? string.Empty : string.Format("LIMIT {0}", top)
                                  );
                        break;
                    }
                case DatabaseStyle.Oracle:
                    {
                        if (0 == top)
                        {
                            sbSelect.AppendFormat("SELECT {0} FROM {1} WHERE 1=1 {2} {3}",
                               sbCols, tableFullName, sbWhere,
                               0 == sbOrderBy.Length ? string.Empty : string.Format("ORDER BY {0}", sbOrderBy));
                        }
                        else
                        {
                            sbSelect.AppendFormat("SELECT A.* FROM (SELECT {0} FROM {1} WHERE 1=1 {2} {3})A WHERE ROWNUM <= {4}",
                                sbCols, tableFullName, sbWhere,
                               0 == sbOrderBy.Length ? string.Empty : string.Format("ORDER BY {0}", sbOrderBy),
                               top);
                        }
                       
                        break;
                    }
                case DatabaseStyle.SqlServer:
                default:
                    {
                        sbSelect.AppendFormat("SELECT {0} {1} FROM {2} WHERE 1=1 {3} {4}",
                                 0 == top ? string.Empty : string.Format("TOP {0}", top),
                                 sbCols, tableFullName, sbWhere,
                                 0 == sbOrderBy.Length ? string.Empty : string.Format("ORDER BY {0}", sbOrderBy));
                        break;
                    }
            }
            
            IFakeCommandAttribute fakeCommand = new FakeCommandAttribute
                                                    {
                                                        CommandText = sbSelect.ToString(),
                                                        Paras = ((List<DbParameter>) paras).ToArray(),
                                                        StorageName = storageAttr.Name,
                                                    };
            return fakeCommand;
        }

        #endregion
    }
}