using Transversal.Application.Dto.Request;

namespace Services.Resources.API.Core.Application.Dto.Resources
{
    public class ResourceRequestDto : LocalizedRequestDtoBase,
        ILocalizedRequestDto
    {
        public int? WithId { get; set; }
        public string WithKey { get; set; }
        public bool? MustBePublic { get; set; }
    }
}
