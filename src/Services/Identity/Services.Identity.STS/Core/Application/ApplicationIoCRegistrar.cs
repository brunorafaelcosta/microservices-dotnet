using Transversal.Common.InversionOfControl;
using Transversal.Common.InversionOfControl.Registrar;

namespace Services.Identity.STS.Core.Application
{
    public class ApplicationIoCRegistrar : IIoCRegistrar
    {
        public void Run(IIoCManager ioCManager)
        {
            ioCManager.RegisterIfNot<IUsersApplicationService, UsersApplicationService>();
        }
    }
}
