using Autofac;
using Autofac.Builder;
using Autofac.Core;
using System;
using System.Collections.Generic;
using Transversal.Common.InversionOfControl;
using Transversal.Common.InversionOfControl.Registrar;

namespace Transversal.Web.InversionOfControl
{
    public class AutofacIoCManager : IIoCManager
    {
        #region Fields

        protected bool _isInitialized;
        protected List<IIoCRegistrar> _ioCRegistrars;
        protected ContainerBuilder _containerBuilder;
        protected ILifetimeScope _lifetimeScope;

        #endregion Fields

        #region Ctor

        public AutofacIoCManager(ContainerBuilder containerBuilder)
        {
            _isInitialized = false;
            _containerBuilder = containerBuilder ?? new ContainerBuilder();
            _containerBuilder.RegisterInstance(this)
                .As<IIoCManager>()
                .As<IIoCResolver>()
                .SingleInstance();
        }

        #endregion Ctor

        #region Properties

        public virtual bool IsInitialized => _isInitialized;

        #endregion Properties

        #region Methods

        public virtual Action<IIoCManager> RegisterMissingComponents { get; set; }

        public virtual void Initialize(IIoCRegistrarCatalog ioCRegistrarCatalog)
        {
            if (ioCRegistrarCatalog is null)
                throw new ArgumentNullException(nameof(ioCRegistrarCatalog));
            if (_isInitialized)
                throw new InvalidOperationException();

            _ioCRegistrars = new List<IIoCRegistrar>();

            foreach (var ioCRegistrar in ioCRegistrarCatalog.ItemsSortedAccordingToDependencies)
            {
                var instance = ioCRegistrar.Instantiate();
                instance.Run(this);
                _ioCRegistrars.Add(instance);
            }

            RegisterMissingComponents?.Invoke(this);

            _containerBuilder.RegisterBuildCallback((lifetimeScope) => { _lifetimeScope = lifetimeScope; });

            _isInitialized = true;
        }

        #region IIoCRegistrar

        public virtual void Register<T>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
            where T : class
        {
            RegisterType(typeof(T), null, lifeStyle);
        }

        public virtual void Register(Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
        {
            RegisterType(type, null, lifeStyle);
        }

        public virtual void Register<TType, TImplementation>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
            where TType : notnull
            where TImplementation : class, TType
        {
            RegisterType(typeof(TType), typeof(TImplementation), lifeStyle);
        }

        public virtual void Register(Type type, Type implementation, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
        {
            RegisterType(type, implementation, lifeStyle);
        }

        public virtual void Register<TType>(Func<IIoCResolver, TType> @delegate, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
            where TType : notnull
        {
            if (_isInitialized)
                throw new InvalidOperationException();

            var registrationBuilder = _containerBuilder
                .Register(c =>
                {
                    var ioCResolver = c.Resolve<IIoCResolver>();
                    return @delegate(ioCResolver);
                })
                .As<TType>();

            SetLifeStyle(registrationBuilder, lifeStyle);
        }

        public virtual void RegisterGeneric(Type type, Type impl, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
        {
            if (_isInitialized)
                throw new InvalidOperationException();

            if (impl is null)
            {
                var _asSelf = _containerBuilder.RegisterGeneric(type).AsSelf();
                SetLifeStyle(_asSelf, lifeStyle);
            }
            else
            {
                var _asType = _containerBuilder.RegisterGeneric(impl).As(type);
                SetLifeStyle(_asType, lifeStyle);
            }
        }

        public virtual void RegisterInstance<TType>(TType instance)
            where TType : class
        {
            if (_isInitialized)
                throw new InvalidOperationException();

            _containerBuilder.RegisterInstance(instance);
        }

        protected virtual void RegisterType(Type type, Type implementation = null, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
        {
            if (_isInitialized)
                throw new InvalidOperationException();

            if (implementation is null)
            {
                var _asSelf = _containerBuilder.RegisterType(type).AsSelf();
                SetLifeStyle(_asSelf, lifeStyle);
            }
            else
            {
                var _asType = _containerBuilder.RegisterType(implementation).As(type);
                SetLifeStyle(_asType, lifeStyle);
            }
        }

        protected virtual void SetLifeStyle<TLimit, TActivatorData, TRegistrationStyle>(IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
        {
            switch (lifeStyle)
            {
                case DependencyLifeStyle.Singleton:
                    registrationBuilder.SingleInstance();
                    break;

                case DependencyLifeStyle.SingletonPerRequest:
                    registrationBuilder.InstancePerLifetimeScope();
                    break;

                default:
                case DependencyLifeStyle.Transient:
                    registrationBuilder.InstancePerDependency();
                    break;
            }
        }

        #endregion

        public virtual bool IsRegistered<TType>()
        {
            return IsRegistered(typeof(TType));
        }

        public virtual bool IsRegistered(Type type)
        {
            if (_isInitialized)
                return _lifetimeScope.IsRegistered(type);
            else
                return _containerBuilder.ComponentRegistryBuilder.IsRegistered(new TypedService(type));
        }

        #region IIoCResolver

        public virtual T Resolve<T>()
        {
            return _lifetimeScope.Resolve<T>();
        }

        public virtual T Resolve<T>(IDictionary<string, object> parameters)
        {
            return _lifetimeScope.Resolve<T>(GetNamedPropertyParameters(parameters));
        }

        public virtual object Resolve(Type type)
        {
            return _lifetimeScope.Resolve(type);
        }

        public virtual object Resolve(Type type, IDictionary<string, object> parameters)
        {
            return _lifetimeScope.Resolve(type, GetNamedPropertyParameters(parameters));
        }

        public virtual void Release(object obj)
        {
        }

        protected virtual IEnumerable<NamedPropertyParameter> GetNamedPropertyParameters(IDictionary<string, object> parameters)
        {
            var namedPropertyParameters = new List<Autofac.Core.NamedPropertyParameter>();
            foreach (var parameter in parameters ?? new Dictionary<string, object>())
            {
                namedPropertyParameters.Add(new Autofac.Core.NamedPropertyParameter(parameter.Key, parameter.Value));
            }
            return namedPropertyParameters;
        }

        #endregion

        #endregion Methods

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
