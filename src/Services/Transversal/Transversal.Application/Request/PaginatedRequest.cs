using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    public class PaginatedRequest<TDto> : ListRequest<TDto>,
        IPaginatedRequest<TDto>
        where TDto : IDto
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
