using Transversal.Common.Dependency;
using Transversal.Data.EFCore;
using Transversal.Data.EFCore.Uow;
using Transversal.Domain.Uow;

namespace Services.Localization.API.Core.Data.Uow
{
    public class UnitOfWork : Transversal.Data.EFCore.Uow.EfCoreUnitOfWork
    {
        public UnitOfWork(
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
