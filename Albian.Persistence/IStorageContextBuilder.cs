#region

using System.Collections.Generic;
using Albian.Persistence.Context;

#endregion

namespace Albian.Persistence
{
    public interface IStorageContextBuilder
    {
        IStorageContext BuildStorageContext<T>(T target)
            where T : IAlbianObject;

        IStorageContext BuildStorageContext<T>(IList<T> target)
            where T : IAlbianObject;

        IDictionary<string, IStorageContext> BuildStorageContexts<T>(T target)
            where T : IAlbianObject;

        IDictionary<string, IStorageContext> BuildStorageContexts<T>(IList<T> target)
            where T : IAlbianObject;
    }
}