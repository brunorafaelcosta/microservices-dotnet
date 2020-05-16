using Services.Resources.API.Core.Domain;
using Transversal.Data.EFCore.Repositories;
using Transversal.Data.EFCore.DbContext;

namespace Services.Resources.API.Core.Data.Repositories
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
