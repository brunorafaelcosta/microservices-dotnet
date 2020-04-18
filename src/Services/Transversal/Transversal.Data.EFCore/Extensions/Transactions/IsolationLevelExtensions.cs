using System.Data;
using Transversal.Common.Exceptions;

namespace Transversal.Data.EFCore.Extensions.Transactions
{
    /// <summary>
    /// Extension methods for <see cref="System.Transactions.IsolationLevel"/> objects.
    /// </summary>
    public static class IsolationLevelExtensions
    {
        /// <summary>
        /// Converts <see cref="System.Transactions.IsolationLevel"/> to <see cref="System.Data.IsolationLevel"/>.
        /// </summary>
        public static System.Data.IsolationLevel ToSystemDataIsolationLevel(this System.Transactions.IsolationLevel isolationLevel)
        {
            switch (isolationLevel)
            {
                case System.Transactions.IsolationLevel.Chaos:
                    return IsolationLevel.Chaos;
                case System.Transactions.IsolationLevel.ReadCommitted:
                    return IsolationLevel.ReadCommitted;
                case System.Transactions.IsolationLevel.ReadUncommitted:
                    return IsolationLevel.ReadUncommitted;
                case System.Transactions.IsolationLevel.RepeatableRead:
                    return IsolationLevel.RepeatableRead;
                case System.Transactions.IsolationLevel.Serializable:
                    return IsolationLevel.Serializable;
                case System.Transactions.IsolationLevel.Snapshot:
                    return IsolationLevel.Snapshot;
                case System.Transactions.IsolationLevel.Unspecified:
                    return IsolationLevel.Unspecified;
                default:
                    throw new DefaultException("Unknown isolation level: " + isolationLevel);
            }
        }
    }
}