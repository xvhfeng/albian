#region

using System.Collections.Generic;
using Albian.Persistence.Context;
using Albian.Persistence.Model;

#endregion

namespace Albian.Persistence.Imp.Command
{
    public interface IStorageContextBuilder
    {
        IDictionary<string, IStorageContext> GenerateStorageContexts<T>(T target,
                                                                        BuildFakeCommandByRoutingsHandler<T>
                                                                            buildFakeCommandByRoutingsHandler,
                                                                        BuildFakeCommandByRoutingHandler<T>
                                                                            buildFakeCommandByRoutingHandler)
            where T : IAlbianObject;

        IDictionary<string, IStorageContext> GenerateStorageContexts<T>(string rountingName, int top,
                                                                        IFilterCondition[] where,
                                                                        IOrderByCondition[] orderby)
            where T : class, IAlbianObject,new();
    }
}