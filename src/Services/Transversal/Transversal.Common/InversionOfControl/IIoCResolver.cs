using System;
using System.Collections.Generic;

namespace Transversal.Common.InversionOfControl
{
    /// <summary>
    /// Dependency injection resolver methods.
    /// </summary>
    public interface IIoCResolver
    {
        /// <summary>
        /// Resolve the given type
        /// </summary> 
        /// <typeparam name="T">Type of the object to resolve</typeparam>
        /// <returns>Resolved object instance</returns>
        T Resolve<T>();

        /// <summary>
        /// Resolve the given type
        /// </summary> 
        /// <typeparam name="T">Type of the object to resolve</typeparam>
        /// <param name="parameters">Constructor parameters</param>
        /// <returns>Resolved object instance</returns>
        T Resolve<T>(IDictionary<string, object> parameters);

        /// <summary>
        /// Resolve the given type
        /// </summary> 
        /// <param name="type">Type of the object to resolve</param>
        /// <returns>Resolved object instance</returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolve the given type
        /// </summary> 
        /// <param name="type">Type of the object to resolve</param>
        /// <param name="parameters">Constructor parameters</param>
        /// <returns>Resolved object instance</returns>
        object Resolve(Type type, IDictionary<string, object> parameters);

        /// <summary>
        /// Checks whether the given type is registered within the container
        /// </summary>
        /// <typeparam name="T">Type to check</typeparam>
        bool IsRegistered<T>();

        /// <summary>
        /// Checks whether the given type is registered within the container
        /// </summary>
        /// <param name="type">Type to check</param>
        bool IsRegistered(Type type);

        /// <summary>
        /// Releases the given object from memory
        /// </summary>
        /// <param name="obj">Object instance</param>
        void Release(object obj);
    }
}
