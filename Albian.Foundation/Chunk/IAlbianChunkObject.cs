using System;
namespace Albian.Foundation.Chunk
{
    public interface IAlbianChunkObject
    {
        string Key { get; set; }

        object Value { get; set; }

        DateTime CreateTime { get; set; }
    }
}