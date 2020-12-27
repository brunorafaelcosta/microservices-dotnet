using Microsoft.Extensions.Logging;
using System;
using Transversal.Common.InversionOfControl;
using Transversal.Common.InversionOfControl.Registrar;
using Transversal.Common.Logging;
using Transversal.Common.Session;

namespace Transversal.Core
{
    public abstract class BootstrapperBase : IBootstrapper, IDisposable
    {
        #region Fields

        protected IIoCManager _ioCManager;
        protected IIoCRegistrarCatalog _ioCRegistrarCatalog;

        protected IBootstrapperSettings _bootstrapperSettings;

        protected ILoggerManager _loggerManager;
        protected ILogger<BootstrapperBase> _logger;
        
        #endregion Fields

        #region Methods

        public abstract IIoCRegistrarCatalog GetIoCRegistrarCatalog();

        public virtual void ConfigureIoC(IIoCManager ioCManager)
        {
            _ioCManager = ioCManager ?? throw new ArgumentNullException(nameof(ioCManager));
            _ioCRegistrarCatalog = GetIoCRegistrarCatalog() ?? throw new NullReferenceException();

            _ioCManager.RegisterMissingComponents = RegisterMissingComponents;

            _ioCManager.Initialize(_ioCRegistrarCatalog);
        }

        public virtual void Bootstrap()
        {
            ConfigureLoggerManager();

            OnBootstrapCompleted();
        }

        protected virtual void RegisterMissingComponents(IIoCManager ioCManager)
        {
            ioCManager.RegisterIfNot<ISessionInfo, NullSessionInfo>(DependencyLifeStyle.SingletonPerRequest);
        }

        protected virtual void ConfigureLoggerManager()
        {
            if (!_ioCManager.IsRegistered<ILoggerManager>())
                _ioCManager.Register<ILoggerManager, NullLoggerManager>(DependencyLifeStyle.SingletonPerRequest);

            _loggerManager = _ioCManager.Resolve<ILoggerManager>();
            _loggerManager.Initialize();

            _logger = _loggerManager.CreateLogger<BootstrapperBase>();
        }

        #endregion Methods

        #region Events

        protected virtual void OnBootstrapCompleted() { }

        #endregion Events

        #region IDisposable

        protected virtual bool IsDisposed { get; set; }

        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;
        }

        #endregion IDisposable
    }
}
