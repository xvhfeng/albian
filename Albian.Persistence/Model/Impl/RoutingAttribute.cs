#region

using System;
using Albian.Persistence.Enum;

#endregion

namespace Albian.Persistence.Model.Impl
{
    [Serializable]
    public class RoutingAttribute : IRoutingAttribute
    {
        private string _name = string.Empty;
        private string _owner = string.Empty;
        private string _storage = string.Empty;
        private string _baseTableName = string.Empty;

        #region IRoutingAttribute Members

        /// <summary>
        /// storagecontext名称
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// table名称
        /// </summary>
        public virtual string BaseTableName
        {
            get { return _baseTableName; }
            set { _baseTableName = value; }
        }

        /// <summary>
        /// 所属对象
        /// </summary>
        public virtual string Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public virtual string Storage
        {
            get { return _storage; }
            set { _storage = value; }
        }

        #endregion
    }
}