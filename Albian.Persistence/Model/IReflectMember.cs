#region

using System.Collections.Generic;

#endregion

namespace Albian.Persistence.Model
{
    public interface IReflectMember
    {
        /// <summary>
        /// 反射类的所有属性成员
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        IDictionary<string, IMemberAttribute> ReflectMembers(string typeFullName, out string defaultTableName);

        /// <summary>
        /// 反射属性成员
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        //IMemberAttribute ReflectProperty(PropertyInfo propertyInfo);
    }
}