using Services.Resources.API.Core.Domain;
using Transversal.Data.EFCore.Repositories;
using Transversal.Data.EFCore.DbContext;

namespace Services.Resources.API.Core.Data.Repositories
{
    public class ResourceRepository : EFCoreRepositoryBase<DefaultDbContext, Resource, int>,
        IResourceRepository
    {
        public ResourceRepository(IDbContextProvider<DefaultDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
