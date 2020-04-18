using System;
using System.Threading.Tasks;

namespace Transversal.Domain.Uow
{
    /// <summary>
    /// Used to complete a Unit of Work.
    /// </summary>
    /// <remarks>
    /// This interface can not be injected or directly used.
    /// Use <see cref="IUnitOfWorkManager"/> instead.
    /// </remarks>
    public interface IUnitOfWorkCompleteHandle : IDisposable
    {
        /// <summary>
        /// Completes this Unit of Work.
        /// It saves all changes and commits transaction if exists.
        /// </summary>
        void Complete();

        /// <summary>
        /// Completes this Unit of Work.
        /// It saves all changes and commits transaction if exists.
        /// </summary>
        Task CompleteAsync();
    }
}
