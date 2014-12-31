using System;

namespace Albian.Foundation.Cached
{
    public class AlbianCachedException : Exception
    {
        public AlbianCachedException(string message) : base(message)
        {
        }

        public AlbianCachedException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
        public AlbianCachedException()
            : base()
        {
        }
    }
}