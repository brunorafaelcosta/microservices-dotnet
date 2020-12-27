using Transversal.Common.InversionOfControl;
using Transversal.Common.InversionOfControl.Registrar;

namespace Transversal.Domain
{
    public class TransversalDomainIoCRegistrar : IIoCRegistrar
    {
        public void Run(IIoCManager ioCManager)
        {
            ioCManager.RegisterIfNot<Uow.Manager.IUnitOfWorkManager, Uow.Manager.UnitOfWorkManager>();
            ioCManager.RegisterIfNot<Uow.Options.IUnitOfWorkDefaultOptions, Uow.Options.UnitOfWorkDefaultOptions>();
            ioCManager.RegisterIfNot<Uow.Provider.ICurrentUnitOfWorkProvider, Uow.Provider.AsyncLocalCurrentUnitOfWorkProvider>();
        }
    }
}
