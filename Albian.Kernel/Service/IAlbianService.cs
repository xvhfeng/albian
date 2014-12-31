using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albian.Kernel.Service
{
    public interface IAlbianService
    {
        ServiceState State { get; set; }
        void BeforeLoading();
        void Loading();
        void AfterLoading();

        void BeforeUnloading();
        void Unloading();
        void AfterUnloading();
    }
}