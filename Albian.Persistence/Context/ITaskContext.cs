#region

using System.Collections.Generic;

#endregion

namespace Albian.Persistence.Context
{
    public interface ITask
    {
        IDictionary<string, IStorageContext> Context { get; set; }
    }
}