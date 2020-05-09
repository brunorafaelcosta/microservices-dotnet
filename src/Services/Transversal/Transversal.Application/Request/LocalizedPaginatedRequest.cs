using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    public class LocalizedPaginatedRequest<TRequestDto, TResponseDto> : PaginatedRequest<TRequestDto, TResponseDto>,
        ILocalizedPaginatedRequest<TRequestDto, TResponseDto>
        where TRequestDto : IDto
        where TResponseDto : IDto
    {
        public string OverriddenLanguage { get; set; }
    }
}
