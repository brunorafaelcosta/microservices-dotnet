using Services.Resources.API.Core.Application.Dto.Resources;
using Services.Resources.API.Core.Domain;
using System;
using System.Threading.Tasks;
using Transversal.Application;
using Transversal.Application.Dto.Response;
using Transversal.Common.Session;
using Transversal.Domain.Uow.Manager;

namespace Services.Resources.API.Core.Application
{
    public class ResourcesAppService : ApplicationServiceBase,
       IResourcesAppService
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourcesAppService(
            ISessionInfo session,
            IUnitOfWorkManager uowManager,
            IResourceRepository resourceRepository)
            : base(session, uowManager)
        {
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        public Task<ResourceDto> GetAsync(ResourceRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResponseDto<ResourceDto>> GetAllAsync(ResourcePaginatedRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceDto> CreateAsync(CreateResourceDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceDto> UpdateAsync(UpdateResourceDto input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}
