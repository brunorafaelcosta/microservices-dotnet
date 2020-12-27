using System;

namespace Transversal.Common.Disposable
{
    /// <summary>
    /// Used to provide an action when <see cref="IDisposable.Dispose"/> method is called.
    /// </summary>
    public class DisposableAction : IDisposable
    {
        public static readonly DisposableAction Empty = new DisposableAction(null);

        private Action _action;

        public DisposableAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            // Interlocked prevents multiple execution of the _action
            var action = System.Threading.Interlocked.Exchange(ref _action, null);
            action?.Invoke();
        }
    }
}
