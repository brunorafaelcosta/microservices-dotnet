using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    public class PaginatedRequest<TRequestDto, TResponseDto> : ListRequest<TRequestDto, TResponseDto>,
        IPaginatedRequest<TRequestDto, TResponseDto>
        where TRequestDto : IDto
        where TResponseDto : IDto
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
