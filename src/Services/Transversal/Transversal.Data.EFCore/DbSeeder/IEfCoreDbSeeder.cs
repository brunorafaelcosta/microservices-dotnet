using System.Threading.Tasks;
using Transversal.Data.EFCore.DbContext;

namespace Transversal.Data.EFCore.DbSeeder
{
    /// <summary>
    /// Interface used by <see cref="DbMigrator.IEFCoreDbMigrator{TDbContext}"/> to seed the given DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">Type of the DbContext</typeparam>
    public interface IEfCoreDbSeeder<TDbContext>
        where TDbContext : EfCoreDbContextBase
    {
        Task SeedAsync(TDbContext context, int? retry = 0);
    }
}
