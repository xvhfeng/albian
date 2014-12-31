#region

using System.Data;

#endregion

namespace Albian.Persistence.Model
{
    public interface IMemberAttribute
    {
        /// <summary>
        /// 实体属性名
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        string FieldName { get; set; }

        /// <summary>
        /// 是否允许为空
        /// </summary>
        bool AllowNull { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        int Length { get; set; }

        /// <summary>
        /// 主键类型
        /// </summary>
        bool PrimaryKey { get; set; }

        /// <summary>
        /// 数据库字段类型
        /// </summary>
        DbType DBType { get; set; }

        bool IsSave { get; set; }
    }
}