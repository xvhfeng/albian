using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albian.Kernel
{
    public class AlbianKernelException : Exception
    {
        public AlbianKernelException(string message) : base(message)
        {
        }

        public AlbianKernelException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        public AlbianKernelException()
            : base()
        {
        }
    }
}
