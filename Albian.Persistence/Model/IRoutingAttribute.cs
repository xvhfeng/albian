#region

using Albian.Persistence.Enum;

#endregion

namespace Albian.Persistence.Model
{
    /// <summary>
    /// 路由属性
    /// </summary>
    public interface IRoutingAttribute
    {
        /// <summary>
        /// storagecontext名称
        /// </summary>
        string Name { get; set; }

        string StorageName { get; set; }

        /// <summary>
        /// table名称
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// 所属对象
        /// </summary>
        string Owner { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        PermissionMode Permission { get; set; }
    }
}