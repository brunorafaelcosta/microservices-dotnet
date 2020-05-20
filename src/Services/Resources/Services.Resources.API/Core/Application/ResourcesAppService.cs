using Services.Resources.API.Core.Application.Dto.Resources;
using Services.Resources.API.Core.Domain;
using System;
using System.Threading.Tasks;
using Transversal.Application;
using Transversal.Application.Dto.Request;
using Transversal.Application.Dto.Response;
using Transversal.Application.Exceptions;
using Transversal.Common.Session;
using Transversal.Domain.Repositories.Options;
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

        public async Task<ResourceDto> GetAsync(ResourceRequestDto request)
        {
            using (var uow = UowManager.Begin())
            using (UowManager.Current.OverrideCurrentLanguageCode(request.LanguageCode ?? Session.LanguageCode))
            {
                var dto = await _resourceRepository.FirstOrDefaultAsync(
                    x =>
                        (!request.WithId.HasValue || (request.WithId.HasValue && request.WithId.Value == x.Id))
                        && (string.IsNullOrEmpty(request.WithKey) || (!string.IsNullOrEmpty(request.WithKey) && request.WithKey == x.Key))
                        && (!request.MustBePublic.HasValue || (request.MustBePublic.HasValue && request.MustBePublic == !x.ResourceGroup.IsPrivate))
                    ,
                    new GetProjectedOptions<Resource, int, ResourceDto>
                    {
                        Projection = ResourceDto.Projection
                    });

                if (dto is null)
                    throw new AppException();

                return dto;
            }
        }

        public async Task<PaginatedResponseDto<ResourceDto>> GetAllAsync(ResourcePaginatedRequestDto request)
        {
            using (var uow = UowManager.Begin())
            using (UowManager.Current.OverrideCurrentLanguageCode(request.LanguageCode ?? Session.LanguageCode))
            {
                var options = request.ResolveRepositoryGetAllProjectedOptions(ResourceDto.Projection);
                var dtos = await _resourceRepository.GetAllListAsync(
                    x =>
                        (!request.WithId.HasValue || (request.WithId.HasValue && request.WithId.Value == x.Id))
                        && (string.IsNullOrEmpty(request.WithKey) || (!string.IsNullOrEmpty(request.WithKey) && request.WithKey == x.Key))
                        && (!request.MustBePublic.HasValue || (request.MustBePublic.HasValue && request.MustBePublic == !x.ResourceGroup.IsPrivate))
                    , options
                    , out long total);

                if (dtos is null)
                    throw new AppException();

                return new PaginatedResponseDto<ResourceDto>(dtos)
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Total = total
                };
            }
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
