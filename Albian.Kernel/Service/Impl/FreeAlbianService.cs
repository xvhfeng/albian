using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albian.Kernel.Service.Impl
{
    public abstract class FreeAlbianService : IAlbianService
    {
        protected FreeAlbianService()
        {

        }

        public ServiceState State { get; set; }

        public virtual void BeforeLoading()
        {
            State = ServiceState.Normal;
        }

        public virtual void Loading()
        {
            State = ServiceState.Loading;
        }

        public virtual void AfterLoading()
        {
            State = ServiceState.Running;
        }

        public virtual void BeforeUnloading()
        {
        }

        public virtual void Unloading()
        {
            State = ServiceState.Unloading;
        }

        public virtual void AfterUnloading()
        {
            State = ServiceState.Unloaded;
        }
    }
}