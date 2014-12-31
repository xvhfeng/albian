#region

using System;
using Albian.Persistence.Enum;

#endregion

namespace Albian.Persistence.Model
{
    public interface IFilterCondition
    {
        RelationalOperators Relational { get; set; }
        string PropertyName { get; set; }
        object Value { get; set; }
        Type Type { get; set; }
        LogicalOperation Logical { get; set; }
    }
}