using System;
using System.Collections.Generic;
using System.Reflection;

namespace Transversal.Common.Catalog
{
    /// <summary>
    /// Interface implemented by the types that will be registered in <see cref="ICatalog{TCatalogItem}"/>.
    /// </summary>
    public interface ICatalogItem
    {
        /// <summary>
        /// Item <see cref="Assembly"/>
        /// </summary>
        Assembly ItemAssembly { get; }

        /// <summary>
        /// Item <see cref="Type"/>
        /// </summary>
        Type ItemType { get; }

        /// <summary>
        /// List of all the types that this item depends upon
        /// </summary>
        IReadOnlyCollection<ICatalogItem> Dependencies { get; }
    }
}
