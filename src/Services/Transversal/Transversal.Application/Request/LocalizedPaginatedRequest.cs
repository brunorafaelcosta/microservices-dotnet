using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    public class LocalizedPaginatedRequest<TDto> : PaginatedRequest<TDto>,
        ILocalizedPaginatedRequest<TDto>
        where TDto : IDto
    {
        public string Language { get; set; }
    }
}
