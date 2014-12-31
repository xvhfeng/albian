#region

using System.Collections.Generic;
using Albian.Persistence.Context;
using Albian.Persistence.Model;

#endregion

namespace Albian.Persistence
{
    public interface ITaskBuilder
    {
        ITask BuildCreateTask<T>(T target)
            where T : IAlbianObject;

        ITask BuildCreateTask<T>(IList<T> target)
            where T : IAlbianObject;

        ITask BuildModifyTask<T>(IList<T> target)
            where T : IAlbianObject;

        ITask BuildModifyTask<T>(T target)
            where T : IAlbianObject;

        ITask BuildRemoveTask<T>(IList<T> target)
            where T : IAlbianObject;

        ITask BuildRemoveTask<T>(T target)
            where T : IAlbianObject;

        ITask BuildSaveTask<T>(IList<T> target)
            where T : IAlbianObject;

        ITask BuildSaveTask<T>(T target)
            where T : IAlbianObject;

        ITask BuildQueryTask<T>(string rountingName, int top, IFilterCondition[] where, IOrderByCondition[] orderby)
            where T : class, IAlbianObject,new();
    }
}