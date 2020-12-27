using System;
using Transversal.Common.Catalog;

namespace Transversal.Common.InversionOfControl.Registrar
{
    /// <summary>
    /// Specifies that the current registrar has a dependency on another registrar.
    /// <para>This attribute should be used on classes that implement the <see cref="IIoCRegistrar"/> interface.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class IoCRegistrarDependencyAttribute : Attribute, ICatalogItemDependencyAttribute
    {
        readonly Type _dependentRegistrarType;

        /// <summary>
        /// Creates a new instance of <see cref="IoCRegistrarDependencyAttribute"/>
        /// </summary>
        /// <param name="registrarType"><see cref="Type"/> of the registrar</param>
        public IoCRegistrarDependencyAttribute(Type dependentRegistrarType)
        {
            _dependentRegistrarType = dependentRegistrarType;
        }

        /// <summary>
        /// The <see cref="Type"/> of the registrar that this registrar is dependant on
        /// </summary>
        public Type DependentType => _dependentRegistrarType;
    }
}
