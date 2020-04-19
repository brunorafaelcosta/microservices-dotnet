using Transversal.Common.Dependency;
using Transversal.Data.EFCore.DbContext;
using Transversal.Data.EFCore.Uow;
using Transversal.Domain.Uow;

namespace Services.Localization.API.Core.Data.Uow
{
    public class DefaultUnityOfWork : EfCoreUnitOfWork
    {
        public DefaultUnityOfWork(
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
