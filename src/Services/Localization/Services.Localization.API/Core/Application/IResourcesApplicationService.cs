using System.Collections.Generic;
using Transversal.Application;

namespace Services.Localization.API.Core.Application
{
    public interface IResourcesApplicationService : IApplicationService
    {
        void CreateResourceGroup();

        IEnumerable<Dto.ResourceGroupDto> GetAllResourceGroups();
    }
}
