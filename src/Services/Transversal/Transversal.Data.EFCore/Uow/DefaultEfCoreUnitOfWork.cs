using Transversal.Common.Dependency;
using Transversal.Data.EFCore.DbContext;
using Transversal.Domain.Uow;

namespace Transversal.Data.EFCore.Uow
{
    public class DefaultEfCoreUnitOfWork : EfCoreUnitOfWork
    {
        public DefaultEfCoreUnitOfWork(
            IIocResolver iocResolver,
            IEfCoreTransactionStrategy transactionStrategy,
            IDbContextResolver dbContextResolver,
            IConnectionStringResolver connectionStringResolver)
            : base(
                iocResolver,
                transactionStrategy,
                dbContextResolver,
                connectionStringResolver)
        {
        }
    }
}
