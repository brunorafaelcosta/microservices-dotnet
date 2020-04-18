using Transversal.Common.Dependency;
using Transversal.Domain.Uow.Options;

namespace Transversal.Data.EFCore.Uow
{
    /// <summary>
    /// Interface used to handle and commit transactions within a Unit Of Work scope.
    /// </summary>
    public interface IEfCoreTransactionStrategy
    {
        void InitOptions(UnitOfWorkOptions options);

        TDbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver)
            where TDbContext : DbContextBase;

        void Commit();

        void Dispose(IIocResolver iocResolver);
    }
}
