#region

using System;
using Albian.Persistence.Enum;

#endregion

namespace Albian.Persistence.Model.Impl
{
    [Serializable]
    public class StorageAttribute : IStorageAttribute
    {
        private DatabaseStyle _databaseStyle = DatabaseStyle.SqlServer;
        private string _databse = string.Empty;
        private bool _integratedSecurity;
        private int _maxPoolSize = 100;
        private int _minPoolSize = 5;
        private string _name = string.Empty;
        private string _password = string.Empty;
        private bool _pooling = true;
        private string _server = string.Empty;
        private int _timeout = 15;
        private string _uid = string.Empty;
        private string _charset = "gb2312";
        private bool _transactional = true;
        private bool _isHealth = true;

        #region IStorageAttribute Members

        public virtual bool Pooling
        {
            get { return _pooling; }
            set { _pooling = value; }
        }

        public virtual bool IntegratedSecurity
        {
            get { return _integratedSecurity; }
            set { _integratedSecurity = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public virtual string Database
        {
            get { return _databse; }
            set { _databse = value; }
        }

        public virtual string Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }

        public virtual string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public virtual int MinPoolSize
        {
            get { return _minPoolSize; }
            set { _minPoolSize = value; }
        }

        public virtual int MaxPoolSize
        {
            get { return _maxPoolSize; }
            set { _maxPoolSize = value; }
        }

        public virtual int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        public virtual DatabaseStyle DatabaseStyle
        {
            get { return _databaseStyle; }
            set { _databaseStyle = value; }
        }

        public virtual string Charset
        {
            get { return _charset; }
            set { _charset = value; }
        }

        public virtual bool Transactional
        {
            get { return _transactional; }
            set { _transactional = value; }
        }

        public virtual bool IsHealth
        {
            get { return _isHealth; }
            set { _isHealth = value; }
        }

        #endregion
    }
}