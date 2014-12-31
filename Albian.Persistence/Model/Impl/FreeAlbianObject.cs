#region

using System;

#endregion

namespace Albian.Persistence.Model.Impl
{
    [Serializable]
    public abstract class FreeAlbianObject : IAlbianObject
    {
        #region Implementation of IAlbianObject

        private string _id = string.Empty;
        private bool _isNew;

        [AlbianMember(PrimaryKey = true)]
        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [AlbianMember(IsSave = false)]
        public virtual bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value; }
        }

        #endregion
    }
}