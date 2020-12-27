using System;

namespace Transversal.Common.Catalog
{
    /// <summary>
    /// Interface implemented by the Attribute used to defined <see cref="ICatalogItem"/> dependencies.
    /// </summary>
    public interface ICatalogItemDependencyAttribute
    {
        /// <summary>
        /// Type of the class holding the dependent <see cref="ICatalogItem"/>
        /// </summary>
        Type DependentType { get; }
    }
}
