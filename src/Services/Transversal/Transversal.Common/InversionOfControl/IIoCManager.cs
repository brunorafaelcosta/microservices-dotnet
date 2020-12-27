using System;
using Transversal.Common.InversionOfControl.Registrar;

namespace Transversal.Common.InversionOfControl
{
    /// <summary>
    /// Interface used to directly perform dependency injection tasks.
    /// </summary>
    public interface IIoCManager : IIoCResolver, IDisposable
    {
        #region Dependency registration

        /// <summary>
        /// Registers a type as self registration
        /// </summary>
        /// <typeparam name="T">Registering type</typeparam>
        /// <param name="lifeStyle">Lifestyle of the objects of this type</param>
        void Register<T>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
            where T : class;

        /// <summary>
        /// Registers a type as self registration
        /// </summary>
        /// <param name="type">Registering type</param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type</param>
        void Register(Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient);

        /// <summary>
        /// Registers a type with its implementation
        /// </summary>
        /// <typeparam name="TType">Registering type</typeparam>
        /// <typeparam name="TImplementation">Type that implements <typeparamref name="TType"/></typeparam>
        /// <param name="lifeStyle">Lifestyle of the objects of this type</param>
        void Register<TType, TImplementation>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
            where TType : notnull
            where TImplementation : class, TType;

        /// <summary>
        /// Registers a type with its implementation
        /// </summary>
        /// <param name="type">Registering type</param>
        /// <param name="implementation">Type that implements <paramref name="type"/></param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type</param>
        void Register(Type type, Type implementation, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient);

        /// <summary>
        /// Registers a type whose implementation is resolved by a delegate
        /// </summary>
        /// <typeparam name="TType">Registering type</typeparam>
        /// <param name="delegate">Delegate used to resolve <typeparamref name="TType"/></param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type</param>
        void Register<TType>(Func<IIoCResolver, TType> @delegate, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
            where TType : notnull;

        /// <summary>
        /// Registers a generic type with its generic implementation
        /// </summary>
        /// <param name="type">Registering type</param>
        /// <param name="implementation">Type that implements <paramref name="type"/></param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type</param>
        void RegisterGeneric(Type type, Type implementation, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient);

        /// <summary>
        /// Registers a existing instance of the given type
        /// </summary>
        /// <typeparam name="TType">Registering type</typeparam>
        /// <param name="instance">Instance object</param>
        void RegisterInstance<TType>(TType instance)
            where TType : class;

        #endregion

        /// <summary>
        /// Indicates if this <see cref="IIoCManager"/> as already been initialized
        /// <para>Once initialized, registration is disabled</para>
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Initialize with the given <see cref="IIoCRegistrar"/> catalog
        /// </summary>
        /// <param name="ioCRegistrarCatalog">Catalog of <see cref="IIoCRegistrar"/></param>
        void Initialize(IIoCRegistrarCatalog ioCRegistrarCatalog);

        /// <summary>
        /// Action called on initialize to register components that are required by the application
        /// but haven't been registered yet
        /// </summary>
        Action<IIoCManager> RegisterMissingComponents { get; set; }
    }
}
