using Microsoft.EntityFrameworkCore;
using Transversal.Data.EFCore.DbContext;
using Transversal.Domain.Uow.Provider;

namespace Services.Identity.STS.Core.Data
{
    public class DefaultDbContext : EfCoreDbContextBase
    {
        public DbSet<Domain.Users.User> Users { get; set; }
        public DbSet<Domain.Users.UserPicture> UserPictures { get; set; }

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