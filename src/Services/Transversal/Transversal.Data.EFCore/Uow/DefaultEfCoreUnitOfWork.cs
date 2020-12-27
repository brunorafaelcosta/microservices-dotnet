using Transversal.Common.InversionOfControl;
using Transversal.Common.Session;
using Transversal.Data.EFCore.DbContext;

namespace Transversal.Data.EFCore.Uow
{
    public class DefaultEfCoreUnitOfWork : EfCoreUnitOfWork
    {
        public DefaultEfCoreUnitOfWork(
            IIoCResolver iocResolver,
            IEfCoreTransactionStrategy transactionStrategy,
            IDbContextResolver dbContextResolver,
            ISessionInfo sessionInfo)
            : base(
                iocResolver,
                transactionStrategy,
                dbContextResolver,
                sessionInfo)
        {
        }
    }
}
