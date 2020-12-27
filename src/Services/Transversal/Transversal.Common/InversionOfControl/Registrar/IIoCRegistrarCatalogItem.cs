using Transversal.Common.Catalog;

namespace Transversal.Common.InversionOfControl.Registrar
{
    public interface IIoCRegistrarCatalogItem : ICatalogItem
    {
        /// <summary>
        /// Gets the <see cref="IIoCRegistrar"/> instance of this catalog item
        /// </summary>
        /// <returns><see cref="IIoCRegistrar"/> instance</returns>
        IIoCRegistrar Instantiate();
    }
}
