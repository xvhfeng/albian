using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albian.Kernel.Service
{
    public class ServiceException: Exception
    {
        public ServiceException(string message) : base(message)
        {
        }

        public ServiceException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        public ServiceException()
            : base()
        {
        }
    }
}