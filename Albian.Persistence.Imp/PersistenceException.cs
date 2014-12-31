#region

using System;

#endregion

namespace Albian.Persistence.Imp
{
    public class PersistenceException : Exception
    {
        public PersistenceException(string message) : base(message)
        {
        }

        public PersistenceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public PersistenceException()
        {
        }
    }
}