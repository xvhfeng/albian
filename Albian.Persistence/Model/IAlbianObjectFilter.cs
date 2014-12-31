#region

using System.Data;
using Albian.Persistence.Enum;

#endregion

namespace Albian.Persistence.Model
{
    public interface IAlbianObjectFilter
    {
        /// <summary>
        /// 查询列的名字
        /// </summary>
        string[] PropertyNames { get; set; }

        /// <summary>
        /// 基表名
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// where条件
        /// </summary>
        string Where { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        SortStyle Orderby { get; set; }

        /// <summary>
        /// 查询参数
        /// </summary>
        IDataParameter[] Paras { get; set; }
    }
}