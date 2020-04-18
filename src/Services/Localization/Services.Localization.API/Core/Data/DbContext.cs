using Microsoft.EntityFrameworkCore;

namespace Services.Localization.API.Core.Data
{
    public class DbContext : Transversal.Data.EFCore.DbContextBase
    {
        public DbSet<Domain.Resources.ResourceGroup> ResourceGroups { get; set; }
        public DbSet<Domain.Resources.Resource> Resource { get; set; }

        public DbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Resources.Resource>(config =>
            {
                config.OwnsOne(p => p.Value);
            });
        }
    }
}
