using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Transversal.Data.EFCore
{
    /// <summary>
    /// Base class for all DbContexts in the application.
    /// </summary>
    public abstract class DbContextBase : DbContext
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        protected DbContextBase(DbContextOptions options)
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
