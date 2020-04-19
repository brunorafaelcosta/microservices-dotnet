using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Transversal.Data.EFCore.DbContext
{
    /// <summary>
    /// Base class for all DbContexts in the application.
    /// </summary>
    public abstract class EfCoreDbContextBase : Microsoft.EntityFrameworkCore.DbContext
    {
        protected EfCoreDbContextBase(DbContextOptions options)
            : base(options)
        {
            InitializeDbContext();
        }

        protected virtual void InitializeDbContext()
        {

        }

        public override int SaveChanges()
        {
            try
            {
                var result = base.SaveChanges();
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Owned<Domain.ValueObjects.Localization.LocalizedValueObject>();
        }
    }
}
