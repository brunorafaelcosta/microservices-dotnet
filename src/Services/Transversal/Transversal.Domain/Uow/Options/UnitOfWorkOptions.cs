using System;
using System.Transactions;

namespace Transversal.Domain.Uow.Options
{
    /// <summary>
    /// Unit of Work options.
    /// </summary>
    public class UnitOfWorkOptions
    {
        public UnitOfWorkOptions()
        {
        }

        /// <summary>
        /// Scope option.
        /// </summary>
        /// <remarks>
        /// Uses default value if not supplied.
        /// </remarks>
        public TransactionScopeOption? Scope { get; set; }

        /// <summary>
        /// Is the Unit of Work transactional?
        /// </summary>
        /// <remarks>
        /// Uses default value if not supplied.
        /// </remarks>
        public bool? IsTransactional { get; set; }

        /// <summary>
        /// Timeout of the Unit of Work in milliseconds.
        /// </summary>
        /// <remarks>
        /// Uses default value if not supplied.
        /// </remarks>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// If the Unit of Work is transactional, this option indicates the isolation level of the transaction.
        /// </summary>
        /// <remarks>
        /// Uses default value if not supplied.
        /// </remarks>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// This option should be set to <see cref="TransactionScopeAsyncFlowOption.Enabled"/>
        /// if the Unit of Work is used in an async scope.
        /// </summary>
        public TransactionScopeAsyncFlowOption? AsyncFlowOption { get; set; }

        internal void FillDefaultsForNonProvidedOptions(IUnitOfWorkDefaultOptions defaultOptions)
        {
            if (!Scope.HasValue)
            {
                Scope = defaultOptions.Scope;
            }

            if (!IsTransactional.HasValue)
            {
                IsTransactional = defaultOptions.IsTransactional;
            }

            if (!Timeout.HasValue && defaultOptions.Timeout.HasValue)
            {
                Timeout = defaultOptions.Timeout.Value;
            }

            if (!IsolationLevel.HasValue && defaultOptions.IsolationLevel.HasValue)
            {
                IsolationLevel = defaultOptions.IsolationLevel.Value;
            }
        }
    }
}
