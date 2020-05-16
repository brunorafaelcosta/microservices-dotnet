using Transversal.Application.Dto.Request;

namespace Services.Resources.API.Core.Application.Dto.Resources
{
    public class ResourcePaginatedRequestDto : BaseLocalizedPaginatedRequestDto<ResourceDto>,
        ILocalizedPaginatedRequestDto<ResourceDto>
    {
        public int? WithId { get; set; }
        public string WithKey { get; set; }
    }
}
