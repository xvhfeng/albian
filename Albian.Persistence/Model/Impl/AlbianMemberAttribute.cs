#region

using System;
using System.Data;

#endregion

namespace Albian.Persistence.Model.Impl
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AlbianMemberAttribute : Attribute, IMemberAttribute
    {
        private bool _allowNull = true;
        private DbType _dbType = DbType.Object;
        private string _fieldName = string.Empty;
        private bool _isSave = true;
        private int _length = 200;
        private string _name = string.Empty;
        private bool _primaryKey;

        #region IMemberAttribute Members

        /// <summary>
        /// 实体属性名
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 字段名
        /// </summary>
        public virtual string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        /// <summary>
        /// 是否允许为空
        /// </summary>
        public virtual bool AllowNull
        {
            get { return _allowNull; }
            set { _allowNull = value; }
        }

        /// <summary>
        /// 字段长度
        /// </summary>
        public virtual int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// 主键类型
        /// </summary>
        public virtual bool PrimaryKey
        {
            get { return _primaryKey; }
            set { _primaryKey = value; }
        }

        /// <summary>
        /// 数据库字段类型
        /// </summary>
        public virtual DbType DBType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        public virtual bool IsSave
        {
            get { return _isSave; }
            set { _isSave = value; }
        }

        #endregion
    }
}