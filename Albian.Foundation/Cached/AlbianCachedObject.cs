using System;
using System.Collections.Generic;
using System.Text;

namespace Albian.Foundation.Cached
{
    public class AlbianCachedObject : IAlbianCachedObject
    {
        public string Key { get; set; }

        public object Value { get; set; }

        public int Timespan { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
