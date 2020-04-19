using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Transversal.Data.EFCore.DbContext
{
    /// <summary>
    /// Interface used by <see cref="IDbContextResolver"/> to get the <see cref="DbContextOptions"> for the given <see cref="TDbContext"/>.
    /// </summary>
    /// <typeparam name="TDbContext">Type of the DbContext</typeparam>
    public interface IDbContextOptionsResolver<TDbContext>
        where TDbContext : EfCoreDbContextBase
    {
        DbContextOptions<TDbContext> GetDbContextOptions(string connectionString);

        DbContextOptions<TDbContext> GetDbContextOptions(string connectionString, DbConnection existingConnection);
    }
}
