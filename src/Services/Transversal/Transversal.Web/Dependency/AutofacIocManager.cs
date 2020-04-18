using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Transversal.Common.Dependency;

namespace Transversal.Web.Dependency
{
    public class AutofacIocManager : IIocManager
    {
        private ILifetimeScope Container { get; set; }

        public AutofacIocManager(ILifetimeScope container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        #region Registrar

        public void Register<T>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
            where T : class
        {
            throw new NotImplementedException();
        }

        public void Register(Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            throw new NotImplementedException();
        }

        public void Register<TType, TImpl>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
            where TType : class
            where TImpl : class, TType
        {
            throw new NotImplementedException();
        }

        public void Register(Type type, Type impl, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type type)
        {
            return Container.IsRegistered(type);
        }

        public bool IsRegistered<TType>()
        {
            return Container.IsRegistered<TType>();
        }

        #endregion Registrar

        #region Resolver

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public T Resolve<T>(Type type)
        {
            return (T)Container.Resolve(type);
        }

        public T Resolve<T>(IDictionary<string, object> arguments)
        {
            var parameters = new List<NamedParameter>();

            foreach (var arg in arguments)
            {
                parameters.Add(new NamedParameter(arg.Key, arg.Value));
            }

            return Container.Resolve<T>(parameters);
        }

        public object Resolve(Type type)
        {
            return Container.Resolve(type);
        }

        public object Resolve(Type type, IDictionary<string, object> arguments)
        {
            var parameters = new List<NamedParameter>();

            foreach (var arg in arguments)
            {
                parameters.Add(new NamedParameter(arg.Key, arg.Value));
            }

            return Container.Resolve(type, parameters);
        }

        public void Release(object obj)
        {
            // throw new NotImplementedException();
        }

        #endregion Resolver

        #region IDisposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Container.Dispose();
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}
