using Services.Localization.API.Core.Application.Dto.ResourceGroups;
using Services.Localization.API.Core.Domain.Resources;
using System;
using Transversal.Application;
using Transversal.Application.Request;
using Transversal.Application.Response;
using Transversal.Common.Session;
using Transversal.Domain.Repositories.Options;
using Transversal.Domain.Uow.Manager;

namespace Services.Localization.API.Core.Application
{
    public class ResourceGroupsAppService : ApplicationServiceBase,
        IResourceGroupsAppService
    {
        private readonly IResourceGroupRepository _resourceGroupRepository;

        public ResourceGroupsAppService(
            ISessionInfo session,
            IUnitOfWorkManager uowManager,
            IResourceGroupRepository resourceGroupRepository)
            : base(session, uowManager)
        {
            _resourceGroupRepository = resourceGroupRepository;
        }

        public IResponse<ResourceGroupDto> Get(int id)
        {
            using (var uow = UowManager.Begin())
            {
                var result = _resourceGroupRepository.Get(
                    id,
                    new GetProjectedOptions<ResourceGroup, int, ResourceGroupDto>
                    {
                        Projection = ResourceGroupDto.Projection
                    });

                return new Response<ResourceGroupDto>(result);
            }
        }

        public IPaginatedResponse<ResourceGroupDto> GetAll(ILocalizedPaginatedRequest<RequestResourceGroupDto, ResourceGroupDto> request)
        {
            PaginatedResponse<ResourceGroupDto> response = new PaginatedResponse<ResourceGroupDto>();

            using (var uow = UowManager.Begin())
            {
                var options = request.ResolveRepositoryGetAllProjectedOptions(ResourceGroupDto.Projection);

                var entities = _resourceGroupRepository.GetAllList(
                    e => e.Name.Contains("Seeded Resource Group"),
                    options,
                    out long totalEntities);

                if (entities != null)
                {
                    response.PageIndex = request.PageIndex;
                    response.PageSize = request.PageSize;
                    response.Total = totalEntities;
                    response.Result = entities.AsReadOnly();
                }
            }

            return response;
        }

        public IResponse<int?> Create(CreateResourceGroupDto input)
        {
            throw new NotImplementedException();
        }

        public IResponse<int> Update(UpdateResourceGroupDto input)
        {
            throw new NotImplementedException();
        }

        public IResponse<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
