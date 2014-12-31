#region

using System;
using System.Collections.Generic;
using System.Data;
using Albian.Persistence.Context;
using Albian.Persistence.Model;

#endregion

namespace Albian.Persistence.Imp.Context
{
    [Serializable]
    public class StorageContext : IStorageContext
    {
        private IList<IDbCommand> _command = new List<IDbCommand>();
        private IDbConnection _connection;
        //private string _connectionString;
        //private IDictionary<string, DbParameter[]> _fakeCommand; // = new Dictionary<string,DbParameter[]>();
        private IList<IFakeCommandAttribute> _fakeCommand;
        private IStorageAttribute _storage;
        private string _storageName;
        private IDbTransaction _transaction;

        #region IStorageContext Members

        public virtual string StorageName
        {
            get { return _storageName; }
            set { _storageName = value; }
        }

        public virtual IList<IFakeCommandAttribute> FakeCommand
        {
            get { return _fakeCommand; }
            set { _fakeCommand = value; }
        }

        public IStorageAttribute Storage
        {
            get { return _storage; }
            set { _storage = value; }
        }

        //public virtual string ConnectionString
        //{
        //    get { return _connectionString; }
        //    set { _connectionString = value; }
        //}

        public virtual IDbConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        public virtual IDbTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        public virtual IList<IDbCommand> Command
        {
            get { return _command; }
            set { _command = value; }
        }

        #endregion
    }
}