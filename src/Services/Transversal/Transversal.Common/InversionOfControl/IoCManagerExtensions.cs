using System;

namespace Transversal.Common.InversionOfControl
{
    /// <summary>
    /// Provides additional methods to <see cref="IIoCManager"/>.
    /// </summary>
    public static class IoCManagerExtensions
    {
        /// <summary>
        /// Registers a type as self registration if isn't already registered
        /// </summary>
        /// <typeparam name="TType">Registering type/typeparam>
        /// <param name="ioCManager"><see cref="IIoCManager"/> instance</param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type</param>
        /// <returns>If the type has been registered</returns>
        public static bool RegisterIfNot<TType>(this IIoCManager ioCManager, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
            where TType : class
        {
            if (ioCManager.IsRegistered<TType>())
            {
                return false;
            }

            ioCManager.Register<TType>(lifeStyle);
            return true;
        }

        /// <summary>
        /// Registers a type with its implementation if isn't already registered
        /// </summary>
        /// <typeparam name="TType">Registering type</typeparam>
        /// <typeparam name="TImplementation">Type that implements <typeparamref name="TType"/></typeparam>
        /// <param name="ioCManager"><see cref="IIoCManager"/> instance</param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type</param>
        /// <returns>If the type has been registered</returns>
        public static bool RegisterIfNot<TType, TImplementation>(this IIoCManager ioCManager, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
            where TType : class
            where TImplementation : class, TType
        {
            if (ioCManager.IsRegistered<TType>())
            {
                return false;
            }

            ioCManager.Register<TType, TImplementation>(lifeStyle);
            return true;
        }

        /// <summary>
        /// Registers a type as self registration if isn't already registered
        /// </summary>
        /// <param name="ioCManager"><see cref="IIoCManager"/> instance</param>
        /// <param name="type">Registering type</param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type</param>
        /// <returns>If the type has been registered</returns>
        public static bool RegisterIfNot(this IIoCManager ioCManager, Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
        {
            if (ioCManager.IsRegistered(type))
            {
                return false;
            }

            ioCManager.Register(type, lifeStyle);
            return true;
        }

        /// <summary>
        /// Registers a type with its implementation if isn't already registered
        /// </summary>
        /// <param name="ioCManager"><see cref="IIoCManager"/> instance</param>
        /// <param name="type">Registering type</param>
        /// <param name="implementation">Type that implements <paramref name="type"/></param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type</param>
        /// <returns>If the type has been registered</returns>
        public static bool RegisterIfNot(this IIoCManager ioCManager, Type type, Type implementation, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
        {
            if (ioCManager.IsRegistered(type))
            {
                return false;
            }

            ioCManager.Register(type, implementation, lifeStyle);
            return true;
        }

        /// <summary>
        /// Registers a generic type with its generic implementation if isn't already registered
        /// </summary>
        /// <param name="ioCManager"><see cref="IIoCManager"/> instance</param>
        /// <param name="type">Registering type</param>
        /// <param name="implementation">Type that implements <paramref name="type"/></param>
        /// <param name="lifeStyle">Lifestyle of the objects of this type</param>
        /// <returns>If the type has been registered</returns>
        public static bool RegisterGenericIfNot(this IIoCManager ioCManager, Type type, Type implementation, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
        {
            if (ioCManager.IsRegistered(type))
            {
                return false;
            }

            ioCManager.RegisterGeneric(type, implementation, lifeStyle);
            return true;
        }
    }
}
