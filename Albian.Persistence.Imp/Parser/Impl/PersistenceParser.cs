#region

using System;
using System.Collections.Generic;
using System.Xml;
using Albian.Kernel;
using Albian.Persistence.Enum;
using Albian.Persistence.Imp.Cache;
using Albian.Persistence.Imp.Reflection;
using Albian.Persistence.Model;
using Albian.Persistence.Model.Impl;

#endregion

namespace Albian.Persistence.Imp.Parser.Impl
{
    public class PersistenceParser : FreePersistenceParser
    {
        public static string DefaultRoutingName
        {
            get { return "DefaultRouting"; }
        }

        public override void Loading()
        {
            Init(@"config/Persistence.config");
            base.Loading();
        }

        protected override IList<IObjectAttribute> ParserObjects(XmlNode entitiesNode)
        {
            if (null == entitiesNode)
            {
                throw new ArgumentNullException("entitiesNode");
            }
            IList<IObjectAttribute> list = new List<IObjectAttribute>();
            XmlNodeList entityNodes = entitiesNode.SelectNodes("AlbianObject");
            if (null == entityNodes || 0 == entityNodes.Count)
            {
                throw new Exception("Parser Object nodes is error in the Persistence.config.");
            }
            foreach (XmlNode node in entityNodes)
            {
                list.Add(ParserObject(node));
            }
            return list;
        }

        protected override IObjectAttribute ParserObject(XmlNode entityNode)
        {
            object imp;
            if (!XmlFileParser.TryGetAttributeValue(entityNode, "Implement", out imp))
            {
                throw new Exception("The implement config item is empty or null in Persistence.config");
            }
            object itf;
            if (!XmlFileParser.TryGetAttributeValue(entityNode, "Interface", out itf))
            {
                throw new Exception("The interface config item is empty or null in Persistence.config");
            }
            IObjectAttribute obj = new AlbianObjectAttribute
                                       {
                                           Implement = imp.ToString(),
                                           Interface = itf.ToString()
                                       };

            //反射解析实体类中属性
            string defaultTableName;
            IReflectMember reflectMember = new ReflectMember();
            obj.MemberAttributes = reflectMember.ReflectMembers(obj.Implement, out defaultTableName);

            if (string.IsNullOrEmpty(defaultTableName))
            {
                throw new Exception("parser the defaultTableName is error.");
            }
            obj.RoutingAttributes = ParserRoutings(defaultTableName, entityNode.SelectNodes("Routings/Routing"));
            obj.MemberAttributes = ParserMembers(obj.Implement, entityNode.SelectNodes("Members/Member"));
            obj.Cache = ParserCache(entityNode.SelectSingleNode("Cache"));
            IDictionary<string, IMemberAttribute> pks = new Dictionary<string, IMemberAttribute>();
            foreach (var member in obj.MemberAttributes)
            {
                if (member.Value.PrimaryKey)
                {
                    pks.Add(member.Value.Name, member.Value);
                }
            }

            if (0 < pks.Count)
            {
                obj.PrimaryKeys = pks;
            }
            ObjectCache.InsertOrUpdate(obj.Implement, obj);
            return obj;
        }

        protected override IDictionary<string, IRoutingAttribute> ParserRoutings(string defaultTableName,
                                                                                 XmlNodeList routingNodes)
        {
            IDictionary<string, IRoutingAttribute> routings = new Dictionary<string, IRoutingAttribute>();
            
            if (null == routingNodes || 0 == routingNodes.Count)
            {
                //set the default value when the routingnodes is not exist

                IRoutingAttribute defaultRouting = new RoutingAttribute
                {
                    Name = DefaultRoutingName,
                    Permission = PermissionMode.WR,
                    StorageName = StorageParser.DefaultStorageName,
                    TableName = defaultTableName,
                };
                routings.Add(DefaultRoutingName, defaultRouting);
                return routings;
            }

            foreach (XmlNode node in routingNodes)
            {
                IRoutingAttribute routing = ParserRouting(defaultTableName, node);
                if (null != routing)
                {
                    routings.Add(routing.Name, routing);
                }
            }
            return routings;
        }

        protected override IRoutingAttribute ParserRouting(string defaultTableName, XmlNode routingNode)
        {
            if (null == routingNode)
            {
                throw new ArgumentNullException("routingNode");
            }
            object name;
            XmlFileParser.TryGetAttributeValue(routingNode, "Name", out name);
            if (null == name)
            {
                throw new Exception("the Name for the routing node is null.");
            }
            object storageName;
            XmlFileParser.TryGetAttributeValue(routingNode, "StorageName", out storageName);
            if (null == storageName)
            {
                throw new Exception("the StorageName for the routing node is null.");
            }
            object tableName;
            XmlFileParser.TryGetAttributeValue(routingNode, "TableName", out tableName);
            object owner;
            XmlFileParser.TryGetAttributeValue(routingNode, "Owner", out owner);
            object oPermission;
            XmlFileParser.TryGetAttributeValue(routingNode, "Permission", out oPermission);

            IRoutingAttribute rounting = new RoutingAttribute
                                             {
                                                 Name = name.ToString(),
                                                 StorageName = storageName.ToString(),
                                                 TableName = null == tableName ? defaultTableName : tableName.ToString(),
                                                 Permission =
                                                     null == oPermission
                                                         ? PermissionMode.WR
                                                         : ConvertToPermissionMode.Convert(oPermission.ToString()),
                                             };
            if (null != owner)
                rounting.Owner = owner.ToString();
            return rounting;
        }

        protected override IDictionary<string, IMemberAttribute> ParserMembers(string typeFullName,
                                                                               XmlNodeList memberNodes)
        {
            if (string.IsNullOrEmpty(typeFullName))
            {
                throw new ArgumentNullException("typeFullName");
            }
            object target = MemberCache.Get(typeFullName);
            if (null == memberNodes || 0 == memberNodes.Count)
            {
                if (null == target)
                {
                    return null;
                }
                return (IDictionary<string, IMemberAttribute>) target;
            }

            foreach (XmlNode node in memberNodes)
            {
                ParserMember(typeFullName, node);
            }

            if (null == target)
            {
                throw new Exception("Get the members attribute is error.");
            }
            return (IDictionary<string, IMemberAttribute>) target;
        }

        protected override IMemberAttribute ParserMember(string typeFullName, XmlNode memberNode)
        {
            if (string.IsNullOrEmpty(typeFullName))
            {
                throw new ArgumentNullException("typeFullName");
            }

            object target = MemberCache.Get(typeFullName);
            if (null == target)
            {
                throw new Exception(string.Format("the member cache is null for {0} type.", typeFullName));
            }
            var members = (IDictionary<string, IMemberAttribute>) target;
            object name;
            XmlFileParser.TryGetAttributeValue(memberNode, "Name", out name);
            if (null == name)
            {
                throw new Exception("there is no name in the member config item.");
            }
            IMemberAttribute member = members[name.ToString()];
            member = members[member.Name] = GenerateMember(member, memberNode);
            MemberCache.InsertOrUpdate(typeFullName, members);
            return member;
        }

        private static IMemberAttribute GenerateMember(IMemberAttribute member, XmlNode memberNode)
        {
            if (null == memberNode)
            {
                return member;
            }
            object oFieldName;
            object oAllowNull;
            object oLength;
            object oPrimaryKey;
            object oDbType;
            object oIsSave;
            if (XmlFileParser.TryGetAttributeValue(memberNode, "FieldName", out oFieldName) && null != oFieldName)
            {
                member.FieldName = oFieldName.ToString().Trim();
            }
            if (XmlFileParser.TryGetAttributeValue(memberNode, "AllowNull", out oAllowNull) && null != oAllowNull)
            {
                member.AllowNull = bool.Parse(oAllowNull.ToString().Trim());
            }
            if (XmlFileParser.TryGetAttributeValue(memberNode, "Length", out oLength) && null != oLength)
            {
                member.Length = int.Parse(oLength.ToString().Trim());
            }
            if (XmlFileParser.TryGetAttributeValue(memberNode, "PrimaryKey", out oPrimaryKey) && null != oPrimaryKey)
            {
                member.PrimaryKey = bool.Parse(oPrimaryKey.ToString().Trim());
                if (member.PrimaryKey) member.AllowNull = false;
            }
            if (XmlFileParser.TryGetAttributeValue(memberNode, "DbType", out oDbType) && null != oDbType)
            {
                member.DBType = ConvertToDbType.Convert(oDbType.ToString());
            }
            if (XmlFileParser.TryGetAttributeValue(memberNode, "IsSave", out oIsSave) && null != oIsSave)
            {
                member.IsSave = bool.Parse(oIsSave.ToString().Trim());
            }
            return member;
        }

        protected override ICacheAttribute ParserCache(XmlNode node)
        {
            if (null == node)
            {
                new CacheAttribute
                    {
                        Enable = true,
                        LifeTime = 300,
                    };
            }
            ICacheAttribute cache = new CacheAttribute();
            object oEnable;
            object oLifeTime;
            if (XmlFileParser.TryGetAttributeValue(node, "Enable", out oEnable))
            {
                cache.Enable = string.IsNullOrEmpty(oEnable.ToString()) ? false : bool.Parse(oEnable.ToString());
            }
            if (XmlFileParser.TryGetAttributeValue(node, "LifeTime", out oLifeTime))
            {
                cache.LifeTime = string.IsNullOrEmpty(oLifeTime.ToString()) ? 0 : int.Parse(oLifeTime.ToString());
            }
            return cache;
        }
    }
}