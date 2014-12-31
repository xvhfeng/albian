#region

using System;
using System.Collections.Generic;
using Albian.Persistence.Context;

#endregion

namespace Albian.Persistence.Imp.Context
{
    [Serializable]
    public class Task : ITask
    {
        private IDictionary<string, IStorageContext> _context = new Dictionary<string, IStorageContext>();

        #region ITask Members

        public virtual IDictionary<string, IStorageContext> Context
        {
            get { return _context; }
            set { _context = value; }
        }

        #endregion
    }
}