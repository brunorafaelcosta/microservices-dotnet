using System;
using System.Transactions;

namespace Transversal.Domain.Uow.Options
{
    /// <summary>
    /// Interface used to get/set Unit of Work default options.
    /// </summary>
    public interface IUnitOfWorkDefaultOptions
    {
        TransactionScopeOption Scope { get; set; }

        bool IsTransactional { get; set; }

        bool IsTransactionScopeAvailable { get; set; }

        TimeSpan? Timeout { get; set; }

        IsolationLevel? IsolationLevel { get; set; }
    }
}
