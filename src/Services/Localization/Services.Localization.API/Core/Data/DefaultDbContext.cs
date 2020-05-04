using Microsoft.EntityFrameworkCore;
using Transversal.Data.EFCore.DbContext;

namespace Services.Localization.API.Core.Data
{
    public class DefaultDbContext : EfCoreDbContextBase
    {
        public DbSet<Domain.Resources.ResourceGroup> ResourceGroups { get; set; }
        public DbSet<Domain.Resources.Resource> Resource { get; set; }

        public DefaultDbContext(DbContextOptions options, IDbContextInterceptor interceptor)
            : base(options, interceptor)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Base method must be called to load the default configuration
            base.OnModelCreating(modelBuilder);
        }
    }
}
