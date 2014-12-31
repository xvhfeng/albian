using System;
namespace Albian.Foundation.Cached
{
    public interface IAlbianCachedObject
    {
        string Key { get; set; }

        object Value { get; set; }

        int Timespan { get; set; }

        DateTime CreateTime { get; set; }
    }
}