using System;
using System.Threading.Tasks;
using Transversal.Common.Exceptions;
using Transversal.Common.Extensions;
using Transversal.Domain.Uow.Options;

namespace Transversal.Domain.Uow
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        private bool _hasBeginBeenCalledBefore;

        private bool _hasCompleteBeenCalledBefore;

        private bool _completedSuccessfully;

        private Exception _failedException;

        protected IConnectionStringResolver ConnectionStringResolver { get; }

        protected UnitOfWorkBase(IConnectionStringResolver connectionStringResolver)
        {
            Id = Guid.NewGuid().ToString("N");

            ConnectionStringResolver = connectionStringResolver;
        }

        public string Id { get; }

        public IUnitOfWork Outer { get; set; }

        public UnitOfWorkOptions Options { get; private set; }

        public bool IsDisposed => disposedValue;

        public void Begin(UnitOfWorkOptions options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            PreventMultipleBegin();
            Options = options;

            BeginUow();
        }

        public void Complete()
        {
            PreventMultipleComplete();
            try
            {
                CompleteUow();
                _completedSuccessfully = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _failedException = ex;
                throw;
            }
        }

        public async Task CompleteAsync()
        {
            PreventMultipleComplete();
            try
            {
                await CompleteUowAsync();
                _completedSuccessfully = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _failedException = ex;
                throw;
            }
        }

        #region Events

        public event EventHandler Completed;

        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        public event EventHandler Disposed;

        protected virtual void OnCompleted()
        {
            Completed.InvokeSafely(this);
        }

        protected virtual void OnFailed(Exception exception)
        {
            Failed.InvokeSafely(this, new UnitOfWorkFailedEventArgs(exception));
        }

        protected virtual void OnDisposed()
        {
            Disposed.InvokeSafely(this);
        }

        #endregion Events

        #region IDisposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_hasBeginBeenCalledBefore || disposedValue)
            {
                return;
            }

            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                disposedValue = true;

                if (!_completedSuccessfully)
                {
                    OnFailed(_failedException);
                }

                DisposeUow();
                OnDisposed();
            }
        }

        ~UnitOfWorkBase()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

        protected abstract void BeginUow();

        protected abstract void CompleteUow();

        protected abstract Task CompleteUowAsync();

        protected abstract void DisposeUow();

        private void PreventMultipleBegin()
        {
            if (_hasBeginBeenCalledBefore)
            {
                throw new DefaultException("This Unit of Work has already been started before. Can not call Begin method more than once.");
            }

            _hasBeginBeenCalledBefore = true;
        }

        private void PreventMultipleComplete()
        {
            if (_hasCompleteBeenCalledBefore)
            {
                throw new DefaultException("This Unit of Work has already been completed. Can not call Complete method more than once.");
            }

            _hasCompleteBeenCalledBefore = true;
        }
    }
}
