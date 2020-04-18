using Services.Localization.API.Core.Domain.Resources;
using Transversal.Data.EFCore.Repositories;
using Transversal.Data.EFCore;

namespace Services.Localization.API.Core.Data.Repositories
{
    public class ResourceGroupRepository : EFCoreRepositoryBase<DbContext, ResourceGroup, int>,
        IResourceGroupRepository
    {
        public ResourceGroupRepository(IDbContextProvider<DbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
