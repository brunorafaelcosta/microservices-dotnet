using Services.Localization.API.Core.Domain.Resources;
using System.Collections.Generic;
using System.Linq;
using Transversal.Application;
using Transversal.Common.Session;
using Transversal.Domain.Uow.Manager;

namespace Services.Localization.API.Core.Application
{
    public class ResourcesApplicationService : ApplicationServiceBase, IResourcesApplicationService
    {
        private readonly IResourceGroupRepository _resourceGroupRepository;

        public ResourcesApplicationService(
            ISessionInfo session,
            IUnitOfWorkManager uowManager,
            IResourceGroupRepository resourceGroupRepository)
            : base(session, uowManager)
        {
            _resourceGroupRepository = resourceGroupRepository;
        }

        public void CreateResourceGroup()
        {
            using (var uow = UowManager.Begin())
            {
                _resourceGroupRepository.Insert(new ResourceGroup()
                {
                    Name = "First Resource Group",
                    Description = "First Resource Group description",
                    IsPrivate = true
                });

                uow.Complete();
            }
        }

        public IEnumerable<Dto.ResourceGroupDto> GetAllResourceGroups()
        {
            using (var uow = UowManager.Begin())
            {
                var resourceGroups = _resourceGroupRepository.GetAllList();

                return resourceGroups.Select(rg => Dto.ResourceGroupDto.FromEntity(rg)).ToList();
            }
        }
    }
}
