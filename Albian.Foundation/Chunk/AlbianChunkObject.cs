using System;
using System.Collections.Generic;
using System.Text;

namespace Albian.Foundation.Chunk
{
    public class AlbianChunkObject : IAlbianChunkObject
    {
        public string Key { get; set; }

        public object Value { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
