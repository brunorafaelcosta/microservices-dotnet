using Transversal.Common.Dependency;
using Transversal.Data.EFCore.DbContext;
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
            where TDbContext : EfCoreDbContextBase;

        void Commit();

        void Dispose(IIocResolver iocResolver);
    }
}
