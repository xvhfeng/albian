using System;

namespace Albian.Foundation.Chunk
{
    public class AlbianChunkException : Exception
    {
        public AlbianChunkException(string message) : base(message)
        {
        }

        public AlbianChunkException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
        public AlbianChunkException()
            : base()
        {
        }
    }
}