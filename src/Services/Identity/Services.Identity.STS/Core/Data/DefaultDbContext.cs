using Microsoft.EntityFrameworkCore;
using Services.Identity.STS.Core.Domain.Users;

namespace Services.Identity.STS.Core.Data
{
    public class DefaultDbContext : Transversal.Data.EFCore.DbContext.EfCoreDbContextBase
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPicture> UserPictures { get; set; }

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