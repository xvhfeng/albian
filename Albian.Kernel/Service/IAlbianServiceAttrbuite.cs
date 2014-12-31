using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albian.Kernel.Service
{
    public interface IAlbianServiceAttrbuite
    {
        string Id { get; set; }
        string Implement { get; set; }
        string Interface { get; set; }
    }
}