using System;
using Transversal.Domain.Uow.Options;

namespace Transversal.Domain.Uow
{
    /// <summary>
    /// Interface implemented by domain Unit of Works.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Manager.IUnitOfWorkManager.Begin()"/> to start a new Unit of Work.
    /// </remarks>
    public interface IUnitOfWork : IUnitOfWorkCompleteHandle, IDisposable
    {
        /// <summary>
        /// Unique id of this Unit of Work.
        /// </summary>
        string Id { get; }
        
        /// <summary>
        /// Reference to the outer Unit of Work if exists.
        /// </summary>
        IUnitOfWork Outer { get; set; }

        /// <summary>
        /// Unit of Work options.
        /// </summary>
        UnitOfWorkOptions Options { get; }

        /// <summary>
        /// Is this Unit of Work disposed?
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Begins the Unit of Work with given options.
        /// </summary>
        /// <param name="options">Unit of Work options</param>
        void Begin(UnitOfWorkOptions options);
        
        #region Events

        /// <summary>
        /// This event is raised when this Unit of Work is successfully completed.
        /// </summary>
        event EventHandler Completed;

        /// <summary>
        /// This event is raised when this Unit of Work is failed.
        /// </summary>
        event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <summary>
        /// This event is raised when this Unit of Work is disposed.
        /// </summary>
        event EventHandler Disposed;

        #endregion Events
    }
}
