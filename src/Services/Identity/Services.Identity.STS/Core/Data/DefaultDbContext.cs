using Microsoft.EntityFrameworkCore;
using Services.Identity.STS.Core.Domain.Users;
using Transversal.Data.EFCore.DbContext;
using Transversal.Domain.Uow.Provider;

namespace Services.Identity.STS.Core.Data
{
    public class DefaultDbContext : EfCoreDbContextBase
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPicture> UserPictures { get; set; }

        public DefaultDbContext(
            DbContextOptions options,
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