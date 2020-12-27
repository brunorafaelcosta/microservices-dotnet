using Microsoft.EntityFrameworkCore;
using Transversal.Data.EFCore.DbContext;
using Transversal.Domain.Uow.Provider;

namespace Services.Resources.API.Core.Data
{
    public class DefaultDbContext : EfCoreDbContextBase
    {
        public DbSet<Domain.ResourceGroup> ResourceGroups { get; set; }
        public DbSet<Domain.Resource> Resource { get; set; }

        public DefaultDbContext(
            DbContextOptions<DefaultDbContext> options,
            IDbContextInterceptor interceptor,
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
            : base(options, interceptor, currentUnitOfWorkProvider)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Base method must be called to load the default configuration
            base.OnModelCreating(modelBuilder);
        }
    }
}
