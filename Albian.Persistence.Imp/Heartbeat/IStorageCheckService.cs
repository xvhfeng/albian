using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Albian.Kernel.Service;

namespace Albian.Persistence.Imp.Heartbeat
{
    public interface IStorageCheckService : IAlbianService
    {
        void Check();
    }
}
