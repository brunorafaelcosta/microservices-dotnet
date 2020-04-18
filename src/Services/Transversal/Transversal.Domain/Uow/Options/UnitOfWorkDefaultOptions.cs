using System;
using System.Transactions;

namespace Transversal.Domain.Uow.Options
{
    public class UnitOfWorkDefaultOptions : IUnitOfWorkDefaultOptions
    {
        public TransactionScopeOption Scope { get; set; }

        public bool IsTransactional { get; set; }

        public TimeSpan? Timeout { get; set; }

        public bool IsTransactionScopeAvailable { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }

        public UnitOfWorkDefaultOptions()
        {
            Scope = TransactionScopeOption.Required;
            IsTransactional = true;
            IsTransactionScopeAvailable = true;
        }
    }
}
