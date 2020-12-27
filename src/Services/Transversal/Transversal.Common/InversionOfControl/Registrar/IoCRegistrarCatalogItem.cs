using System;
using System.Collections.Generic;
using Transversal.Common.Catalog;
using Transversal.Common.Extensions;

namespace Transversal.Common.InversionOfControl.Registrar
{
    public class IoCRegistrarCatalogItem : CatalogItemBase, IIoCRegistrarCatalogItem
    {
        public IoCRegistrarCatalogItem(Type itemType, IEnumerable<ICatalogItem> itemDependencies)
            : base(itemType, itemDependencies)
        {
        }

        public IIoCRegistrar Instantiate()
        {
            return Activator.CreateInstance(ItemType).As<IIoCRegistrar>();
        }
    }
}
