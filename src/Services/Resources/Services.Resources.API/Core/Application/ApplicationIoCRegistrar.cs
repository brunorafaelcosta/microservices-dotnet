using Transversal.Common.InversionOfControl;
using Transversal.Common.InversionOfControl.Registrar;

namespace Services.Resources.API.Core.Application
{
    public class ApplicationIoCRegistrar : IIoCRegistrar
    {
        public void Run(IIoCManager ioCManager)
        {
            ioCManager.RegisterIfNot<IResourcesAppService, ResourcesAppService>();
        }
    }
}
