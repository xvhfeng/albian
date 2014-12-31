#region

using System.Collections.Generic;

#endregion

namespace Albian.Persistence.Model
{
    public interface IObjectAttribute
    {
        /// <summary>
        /// 存储上下文
        /// </summary>
        IDictionary<string, IRoutingAttribute> RoutingAttributes { get; set; }

        /// <summary>
        /// 属性的成员
        /// </summary>
        IDictionary<string, IMemberAttribute> MemberAttributes { get; set; }

        /// <summary>
        /// 主键标识
        /// </summary>
        IDictionary<string, IMemberAttribute> PrimaryKeys { get; set; }

        string Implement { get; set; }

        string Interface { get; set; }

        ICacheAttribute Cache { get; set; }

        IRoutingAttribute RountingTemplate { get; set; }
    }
}