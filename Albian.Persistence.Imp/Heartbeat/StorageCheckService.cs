using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Albian.Kernel.Service.Impl;
using System.Collections;
using Albian.Persistence.Imp.Cache;
using Albian.Persistence.Imp.Command;
using Albian.Persistence.Imp.ConnectionPool;
using Albian.Persistence.Imp.Parser.Impl;
using Albian.Persistence.Model;

namespace Albian.Persistence.Imp.Heartbeat
{
    public class StorageCheckService : FreeAlbianService, IStorageCheckService
    {
        public override void Loading()
        {
            new Thread(Check).Start();
            base.Loading();
        }
        #region Implementation of IStorageCheckService

        public void Check()
        {
            while(true)
            {
                Hashtable ht = UnhealthyStorage.UnhealthyStorages;
                if (0 != ht.Count)
                {
                    foreach (KeyValuePair<string, string> kv in ht)
                    {
                        if (Test(kv.Value))
                        {
                            IStorageAttribute storageAttr = (IStorageAttribute)StorageCache.Get(kv.Value);
                            storageAttr.IsHealth = true;
                        }
                    }
                }
                Thread.Sleep(300 * 1000);
            }
        }

        public bool Test(string storageName)
        {
            IStorageAttribute storageAttr = (IStorageAttribute)StorageCache.Get(storageName);
            string sConnection = StorageParser.BuildConnectionString(storageAttr);
            IDbConnection conn =
                storageAttr.Pooling
                    ?
                        DbConnectionPoolManager.GetConnection(storageName, sConnection)
                    :
                        DatabaseFactory.GetDbConnection(storageAttr.DatabaseStyle, sConnection);
            try
            {
                if (ConnectionState.Open != conn.State)
                    conn.Open();
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT 1 AS Row";
                cmd.CommandType = CommandType.Text;
                object oValue = cmd.ExecuteScalar();
                int value;
                if (int.TryParse(oValue.ToString(), out value) && 1 == value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                return false;
            }
            finally
            {
                if (ConnectionState.Closed != conn.State)
                    conn.Close();
                if (storageAttr.Pooling)
                {
                    DbConnectionPoolManager.RetutnConnection(storageAttr.Name,conn);
                }
                else
                {
                    conn.Dispose();
                    conn = null;
                }
            }

        }

        #endregion
    }
}