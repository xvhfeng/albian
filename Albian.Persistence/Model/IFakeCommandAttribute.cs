#region

using System.Data.Common;

#endregion

namespace Albian.Persistence.Model
{
    public interface IFakeCommandAttribute
    {
        string StorageName { get; set; }
        string CommandText { get; set; }
        DbParameter[] Paras { get; set; }
    }
}