using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albian.Kernel
{
    [AttributeUsage(
        AttributeTargets.Class 
        | AttributeTargets.Delegate 
        | AttributeTargets.Enum
        | AttributeTargets.Interface 
        | AttributeTargets.Struct)]
    public class AlbianKernelAttribute : Attribute
    {
    }
}
