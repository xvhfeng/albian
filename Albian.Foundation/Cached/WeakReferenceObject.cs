using System;
using System.Collections.Generic;
using System.Text;

namespace Albian.Foundation.Cached
{
    public class WeakReferenceObject
    {
        public static WeakReference CreateWeakReferenceObject(object obj)
        {
            return new WeakReference(obj);
        }
    }
}
