using System.Transactions;
using Transversal.Domain.Uow.Options;

namespace Transversal.Domain.Uow.Manager
{
    /// <summary>
    /// Interface used to begin and control a Unit of Work.
    /// </summary>
    public interface IUnitOfWorkManager
    {
        /// <summary>
        /// Gets currently active Unit of Work (or null if doesn't exists).
        /// </summary>
        IUnitOfWork Current { get; }

        /// <summary>
        /// Begins a new Unit of Work.
        /// </summary>
        /// <returns>A handle to be able to complete the Unit of Work</returns>
        IUnitOfWorkCompleteHandle Begin();

        // <summary>
        /// Begins a new Unit of Work.
        /// </summary>
        /// <returns>A handle to be able to complete the Unit of Work</returns>
        IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope);

        // <summary>
        /// Begins a new Unit of Work.
        /// </summary>
        /// <returns>A handle to be able to complete the Unit of Work</returns>
        IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options);
    }
}
