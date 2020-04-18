using System;

namespace Transversal.Domain.Uow
{
    /// <summary>
    /// Used as event arguments for <see cref="IUnitOfWork.Failed"/> event.
    /// </summary>
    public class UnitOfWorkFailedEventArgs : EventArgs
    {
        /// <summary>
        /// Exception that caused the failure.
        /// </summary>
        public Exception Exception { get; private set; }

        public UnitOfWorkFailedEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
