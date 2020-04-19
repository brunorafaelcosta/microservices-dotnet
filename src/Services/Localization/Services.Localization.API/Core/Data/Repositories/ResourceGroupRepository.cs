using Services.Localization.API.Core.Domain.Resources;
using Transversal.Data.EFCore.Repositories;
using Transversal.Data.EFCore.DbContext;

namespace Services.Localization.API.Core.Data.Repositories
{
    public class ResourceGroupRepository : EFCoreRepositoryBase<DefaultDbContext, ResourceGroup, int>,
        IResourceGroupRepository
    {
        public ResourceGroupRepository(IDbContextProvider<DefaultDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
