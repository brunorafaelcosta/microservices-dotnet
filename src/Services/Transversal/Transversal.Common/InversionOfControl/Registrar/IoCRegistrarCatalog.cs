using System;
using System.Collections.Generic;
using Transversal.Common.Catalog;

namespace Transversal.Common.InversionOfControl.Registrar
{
    public class IoCRegistrarCatalog : CatalogBase<IIoCRegistrarCatalogItem, IoCRegistrarDependencyAttribute>, IIoCRegistrarCatalog
    {
        protected override IIoCRegistrarCatalogItem InstantiateCatalogItem(Type itemType, IEnumerable<IIoCRegistrarCatalogItem> itemDependencies)
        {
            return new IoCRegistrarCatalogItem(itemType, itemDependencies);
        }
    }
}
