using Services.Resources.API.Core.Application.Dto.Resources;
using System.Threading.Tasks;
using Transversal.Application;
using Transversal.Application.Dto.Response;

namespace Services.Resources.API.Core.Application
{
    public interface IResourcesAppService : IApplicationService
    {
        Task<ResourceDto> GetAsync(ResourceRequestDto request);

        Task<PaginatedResponseDto<ResourceDto>> GetAllAsync(ResourcePaginatedRequestDto request);

        Task<ResourceDto> CreateAsync(CreateResourceDto input);

        Task<ResourceDto> UpdateAsync(UpdateResourceDto input);

        Task<bool> DeleteAsync(int id);
        
        Task<bool> DeleteAsync(string key);
    }
}
