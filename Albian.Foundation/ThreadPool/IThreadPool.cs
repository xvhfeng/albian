using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Albian.Foundation.ThreadPool
{
    interface IThreadPool : IDisposable
    {
        void QueueUserWorkItem(WaitCallback work, object obj);
    }
}