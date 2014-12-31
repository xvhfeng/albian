#region

using Albian.Persistence.Context;

#endregion

namespace Albian.Persistence.Imp.TransactionCluster
{
    internal interface ITransactionClusterScope
    {
        TransactionClusterState State { get; }
        bool Execute(ITask task);
    }
}