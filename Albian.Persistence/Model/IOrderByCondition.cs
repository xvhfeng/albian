#region

using Albian.Persistence.Enum;

#endregion

namespace Albian.Persistence.Model
{
    public interface IOrderByCondition
    {
        string PropertyName { get; set; }
        SortStyle SortStyle { get; set; }
    }
}