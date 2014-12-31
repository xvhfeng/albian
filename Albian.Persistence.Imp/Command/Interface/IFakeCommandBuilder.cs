#region

using System.Collections.Generic;
using System.Reflection;
using Albian.Persistence.Context;
using Albian.Persistence.Enum;
using Albian.Persistence.Model;

#endregion

namespace Albian.Persistence.Imp.Command
{
    public delegate IDictionary<string, IStorageContext> GenerateStorageContextsHandler<T>(T target,
                                                                                           BuildFakeCommandByRoutingsHandler
                                                                                               <T>
                                                                                               buildFakeCommandByRoutingsHandler,
                                                                                           BuildFakeCommandByRoutingHandler
                                                                                               <T>
                                                                                               buildFakeCommandByRoutingHandler
        )
        where T : IAlbianObject;

    public delegate IDictionary<string, IStorageContext> BuildFakeCommandByRoutingsHandler<T>(T target,
                                                                                              PropertyInfo[] properties,
                                                                                              IObjectAttribute
                                                                                                  objectAttribute,
                                                                                              BuildFakeCommandByRoutingHandler
                                                                                                  <T> routingHandler)
        where T : IAlbianObject;

    public delegate IFakeCommandAttribute BuildFakeCommandByRoutingHandler<T>(PermissionMode permission, T target,
                                                                              IRoutingAttribute routing,
                                                                              IObjectAttribute objectAttribute,
                                                                              PropertyInfo[] properties)
        where T : IAlbianObject;


    public interface IFakeCommandBuilder
    {
        IDictionary<string, IStorageContext> GenerateFakeCommandByRoutings<T>(T target, PropertyInfo[] properties,
                                                                              IObjectAttribute objectAttribute,
                                                                              BuildFakeCommandByRoutingHandler<T>
                                                                                  buildFakeCommandByRoutingHandler)
            where T : IAlbianObject;

        IFakeCommandAttribute BuildCreateFakeCommandByRouting<T>(PermissionMode permission, T target,
                                                                 IRoutingAttribute routing,
                                                                 IObjectAttribute objectAttribute,
                                                                 PropertyInfo[] properties)
            where T : IAlbianObject;

        IFakeCommandAttribute BuildModifyFakeCommandByRouting<T>(PermissionMode permission, T target,
                                                                 IRoutingAttribute routing,
                                                                 IObjectAttribute objectAttribute,
                                                                 PropertyInfo[] properties)
            where T : IAlbianObject;

        IFakeCommandAttribute BuildDeleteFakeCommandByRouting<T>(PermissionMode permission, T target,
                                                                 IRoutingAttribute routing,
                                                                 IObjectAttribute objectAttribute,
                                                                 PropertyInfo[] properties)
            where T : IAlbianObject;

        IFakeCommandAttribute BuildSaveFakeCommandByRouting<T>(PermissionMode permission, T target,
                                                               IRoutingAttribute routing,
                                                               IObjectAttribute objectAttribute,
                                                               PropertyInfo[] properties)
            where T : IAlbianObject;

        IFakeCommandAttribute GenerateQuery<T>(string rountingName, int top, IFilterCondition[] where,
                                               IOrderByCondition[] orderby)
            where T :class, IAlbianObject,new();
    }
}