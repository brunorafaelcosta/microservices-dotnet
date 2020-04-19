using Transversal.Data.EFCore.DbContext;

namespace Transversal.Data.EFCore.DbMigrator
{
    /// <summary>
    /// Interface used to ensure that the given DbContext is created and all migrations are applied.
    /// </summary>
    /// <typeparam name="TDbContext">Type of the DbContext</typeparam>
    public interface IEFCoreDbMigrator<TDbContext>
        where TDbContext : EfCoreDbContextBase
    {
        void CreateOrMigrate();
    }
}
