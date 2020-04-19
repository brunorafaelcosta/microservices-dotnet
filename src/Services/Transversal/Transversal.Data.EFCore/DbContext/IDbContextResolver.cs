using System.Data.Common;

namespace Transversal.Data.EFCore.DbContext
{
    /// <summary>
    /// Interface used by <see cref="Uow.EfCoreUnitOfWork"/> to resolve (instantiate) a DbContext.
    /// </summary>
    public interface IDbContextResolver
    {
        TDbContext Resolve<TDbContext>(string connectionString, DbConnection existingConnection)
            where TDbContext : EfCoreDbContextBase;
    }
}
