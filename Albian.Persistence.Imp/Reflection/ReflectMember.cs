#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Albian.Persistence.Imp.Cache;
using Albian.Persistence.Model;
using Albian.Persistence.Model.Impl;

#endregion

namespace Albian.Persistence.Imp.Reflection
{
    public class ReflectMember : IReflectMember
    {
        #region IReflectMember Members

        public IDictionary<string, IMemberAttribute> ReflectMembers(string typeFullName, out string defaultTableName)
        {
            //string typeFullName = entity.Implement;
            if (string.IsNullOrEmpty(typeFullName))
            {
                throw new ArgumentNullException("typeFullName");
            }
            Type type = Type.GetType(typeFullName, true);
            defaultTableName = type.Name;
            PropertyInfo[] propertyInfos =
                type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            //the interface
            Type itf = type.GetInterface("IAlbianObject"); //get the signature interface
            PropertyInfo idInfo = itf.GetProperty("Id");
            PropertyInfo isNewInfo = itf.GetProperty("IsNew");

            if (null == propertyInfos || 0 == propertyInfos.Length)
            {
                throw new Exception(string.Format("Reflect the {0} property is error.", typeFullName));
            }

            IDictionary<string, IMemberAttribute> memberAttributes = new Dictionary<string, IMemberAttribute>();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                IMemberAttribute memberAttribute = ReflectProperty(propertyInfo, idInfo, isNewInfo);
                memberAttributes.Add(memberAttribute.Name, memberAttribute);
            }
            if (0 == memberAttributes.Count)
            {
                return null;
            }

            PropertyCache.InsertOrUpdate(typeFullName, propertyInfos); //just only have property 
            MemberCache.InsertOrUpdate(typeFullName, memberAttributes); //parser object by member attributes
            return memberAttributes;
        }

        #endregion

        public IMemberAttribute ReflectProperty(PropertyInfo propertyInfo, PropertyInfo idInfo, PropertyInfo isNewInfo)
        {
            object[] attrs = propertyInfo.GetCustomAttributes(typeof (AlbianMemberAttribute), true);

            if (propertyInfo.Name == idInfo.Name)
            {
                if (null == attrs || 0 == attrs.Length)
                    attrs = idInfo.GetCustomAttributes(typeof (AlbianMemberAttribute), true);
                ;
            }
            if (propertyInfo.Name == isNewInfo.Name)
            {
                if (null == attrs || 0 == attrs.Length)
                    attrs = isNewInfo.GetCustomAttributes(typeof (AlbianMemberAttribute), true);
                ;
            }

            IMemberAttribute memberAttribute;
            if (null == attrs || 0 == attrs.Length)
            {
                memberAttribute = new AlbianMemberAttribute
                                      {
                                          Name = propertyInfo.Name,
                                          PrimaryKey = false,
                                          FieldName = propertyInfo.Name,
                                          DBType = ConvertToDbType.Convert(propertyInfo.PropertyType),
                                          IsSave = true,
                                          AllowNull = Utils.IsNullableType(propertyInfo.PropertyType)
                                      };
            }
            else
            {
                AlbianMemberAttribute attr = (AlbianMemberAttribute) attrs[0];
                memberAttribute = new AlbianMemberAttribute();
                if (string.IsNullOrEmpty(attr.FieldName))
                    memberAttribute.FieldName = propertyInfo.Name;
                if (string.IsNullOrEmpty(attr.Name))
                    memberAttribute.Name = propertyInfo.Name;

                //it have problem possible
                memberAttribute.AllowNull = attr.AllowNull
                                                ? Utils.IsNullableType(propertyInfo.PropertyType) ? true : false
                                                : false;
                memberAttribute.DBType = DbType.Object == attr.DBType
                                             ? ConvertToDbType.Convert(propertyInfo.PropertyType)
                                             : attr.DBType;
                memberAttribute.IsSave = attr.IsSave;
                memberAttribute.Length = attr.Length;
                memberAttribute.PrimaryKey = attr.PrimaryKey;
            }
            return memberAttribute;
        }
    }
}