using System;
using System.Collections.Generic;
using System.Linq;
using Transversal.Common.Exceptions;
using Transversal.Common.Extensions.Collections;

namespace Transversal.Common.Catalog
{
    public abstract class CatalogBase<TCatalogItem, TCatalogItemDependencyAttribute> : ICatalog<TCatalogItem>
        where TCatalogItem : ICatalogItem
        where TCatalogItemDependencyAttribute : ICatalogItemDependencyAttribute
    {
        readonly List<TCatalogItem> _registeredItems;

        public CatalogBase()
        {
            _registeredItems = new List<TCatalogItem>();
        }

        public virtual IReadOnlyCollection<TCatalogItem> Items => _registeredItems.AsReadOnly();
        public virtual IReadOnlyCollection<TCatalogItem> ItemsSortedAccordingToDependencies => _registeredItems
            .TopologicalSort(i => i.Dependencies.Cast<TCatalogItem>())
            .ToList()
            .AsReadOnly();

        public virtual TCatalogItem Register<T>()
        {
            return Register(typeof(T));
        }
        public virtual TCatalogItem Register(Type type)
        {
            TCatalogItem item = default;

            // Check if the item has been already registered
            item = _registeredItems.FirstOrDefault(m => m.ItemType == type);

            if (item == null)
            {
                // Resolve all its dependencies
                var itemDependencies = new List<TCatalogItem>();
                foreach (var dependentItem in GetDependentItems(type))
                {
                    var itemDependency = Register(dependentItem);
                    itemDependencies.Add(itemDependency);
                }

                item = InstantiateCatalogItem(type, itemDependencies);

                // Register the item
                _registeredItems.Add(item);
            }

            return item;
        }

        protected virtual IEnumerable<Type> GetDependentItems(Type type)
        {
            if (!type.IsClass || typeof(ICatalogItem).IsAssignableFrom(type))
            {
                throw new DefaultException(type.AssemblyQualifiedName + " isn't a valid " + typeof(ICatalogItem).AssemblyQualifiedName);
            }

            List<Type> dependentItemTypes = new List<Type>();

            if (type.IsDefined(typeof(TCatalogItemDependencyAttribute), true))
            {
                var attributes = type.GetCustomAttributes(typeof(TCatalogItemDependencyAttribute), true).Cast<TCatalogItemDependencyAttribute>();

                dependentItemTypes = attributes.Select(a => a.DependentType).ToList();
            }

            return dependentItemTypes;
        }

        protected abstract TCatalogItem InstantiateCatalogItem(Type itemType, IEnumerable<TCatalogItem> itemDependencies);
    }
}
