using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Albian.Persistence.Imp.Cache
{
    public class UnhealthyStorage
    {
        private static Hashtable _ht = Hashtable.Synchronized(new Hashtable());
        public static void Add(string storageName)
        {
            if (_ht.ContainsKey(storageName))
                return;
            _ht.Add(storageName,storageName);
        }

        public static void Delete(string storageName)
        {
            _ht.Remove(storageName);
        }

        public static Hashtable UnhealthyStorages
        {
            get { return _ht; }
        }
    }
}
