using Transversal.Common.InversionOfControl;
using Transversal.Common.InversionOfControl.Registrar;
using Transversal.Data.EFCore.DbContext;
using Transversal.Data.EFCore.DbMigrator;
using Transversal.Data.EFCore.Uow;

namespace Transversal.Data.EFCore
{
    [IoCRegistrarDependency(typeof(Domain.TransversalDomainIoCRegistrar))]
    public class TransversalDataEFCoreIoCRegistrar : IIoCRegistrar
    {
        public void Run(IIoCManager ioCManager)
        {
            ioCManager.RegisterIfNot<Domain.Uow.IUnitOfWork, DefaultEfCoreUnitOfWork>();
            ioCManager.RegisterIfNot<IConnectionStringResolver, DefaultConnectionStringResolver>();
            ioCManager.RegisterIfNot<IDbContextResolver, DefaultDbContextResolver>();
            ioCManager.RegisterIfNot<IDbContextInterceptor, DefaultDbContextInterceptor>();
            ioCManager.RegisterIfNot<IEfCoreTransactionStrategy, DbContextEfCoreTransactionStrategy>();

            ioCManager.RegisterGenericIfNot(typeof(IEFCoreDbMigrator<>), typeof(DefaultEFCoreDbMigrator<>), DependencyLifeStyle.SingletonPerRequest);
            ioCManager.RegisterGenericIfNot(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>), DependencyLifeStyle.SingletonPerRequest);
        }
    }
}
