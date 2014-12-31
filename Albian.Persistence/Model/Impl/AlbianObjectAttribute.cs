#region

using System;
using System.Collections.Generic;

#endregion

namespace Albian.Persistence.Model.Impl
{
    [Serializable]
    public class AlbianObjectAttribute : IObjectAttribute
    {
        private ICacheAttribute _cache;
        private string _type = string.Empty;
        private IDictionary<string, IMemberAttribute> _memberAttributes;
        private IDictionary<string, IMemberAttribute> _primaryKeys;
        private IRoutingAttribute _defaultRounting;

        #region IObjectAttribute Members

        /// <summary>
        /// 属性的成员
        /// </summary>
        public virtual IDictionary<string, IMemberAttribute> MemberAttributes
        {
            get { return _memberAttributes; }
            set { _memberAttributes = value; }
        }

        public virtual IDictionary<string, IMemberAttribute> PrimaryKeys
        {
            get { return _primaryKeys; }
            set { _primaryKeys = value; }
        }

        public virtual string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public ICacheAttribute Cache
        {
            get { return _cache; }
            set { _cache = value; }
        }

        public virtual IRoutingAttribute DefaultRounting
        {
            get { return _defaultRounting; }
            set { _defaultRounting = value; }
        }

        #endregion
    }
}