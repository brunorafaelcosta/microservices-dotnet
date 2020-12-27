using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Transversal.Common.Catalog
{
    public abstract class CatalogItemBase : ICatalogItem
    {
        readonly List<ICatalogItem> _dependencies;

        public CatalogItemBase(Type itemType, IEnumerable<ICatalogItem> itemDependencies)
        {
            if (itemType == null)
                throw new ArgumentNullException(nameof(itemType));

            ItemAssembly = itemType.Assembly;
            ItemType = itemType;

            _dependencies = itemDependencies?.ToList() ?? new List<ICatalogItem>();
        }

        public virtual Assembly ItemAssembly { get; private set; }

        public virtual Type ItemType { get; private set; }

        public virtual IReadOnlyCollection<ICatalogItem> Dependencies => _dependencies.AsReadOnly();

        public override bool Equals(object obj)
        {
            var item = obj as CatalogItemBase;

            if (item == null)
            {
                return false;
            }

            return ItemType.Equals(item.ItemType);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ItemType);
        }
    }
}
