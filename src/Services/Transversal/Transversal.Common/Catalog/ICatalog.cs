using System;
using System.Collections.Generic;

namespace Transversal.Common.Catalog
{
    /// <summary>
    /// Interface that will hold the registered <typeparamref name="TCatalogItem"/>.
    /// </summary>
    public interface ICatalog<TCatalogItem>
        where TCatalogItem : ICatalogItem
    {
        /// <summary>
        /// List of all the registered items
        /// </summary>
        IReadOnlyCollection<TCatalogItem> Items { get; }

        /// <summary>
        /// List of all the registered items sorted according to the dependencies
        /// </summary>
        IReadOnlyCollection<TCatalogItem> ItemsSortedAccordingToDependencies { get; }

        /// <summary>
        /// Registers a new <see cref="TCatalogItem"/> and resolves all its dependencies
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> of the item</typeparam>
        /// <returns>The registered <see cref="TCatalogItem"/></returns>
        TCatalogItem Register<T>();

        /// <summary>
        /// Registers a new <see cref="TCatalogItem"/> and resolves all its dependencies
        /// </summary>
        /// <param name="type"><see cref="Type"/> of the item</param>
        /// <returns>The registered <see cref="TCatalogItem"/></returns>
        TCatalogItem Register(Type type);
    }
}
