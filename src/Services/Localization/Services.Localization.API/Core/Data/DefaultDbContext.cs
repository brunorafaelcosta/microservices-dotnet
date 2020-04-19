using Microsoft.EntityFrameworkCore;

namespace Services.Localization.API.Core.Data
{
    public class DefaultDbContext : Transversal.Data.EFCore.DbContext.EfCoreDbContextBase
    {
        public DbSet<Domain.Resources.ResourceGroup> ResourceGroups { get; set; }
        public DbSet<Domain.Resources.Resource> Resource { get; set; }

        public DefaultDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Base method must be called to load the default configuration
            base.OnModelCreating(modelBuilder);
        }
    }
}
