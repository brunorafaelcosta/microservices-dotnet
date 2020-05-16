namespace Transversal.Application.Dto.Request
{
    public class BasePaginatedRequestDto<TResponseDto> : BaseListRequestDto<TResponseDto>,
        IPaginatedRequestDto<TResponseDto>
        where TResponseDto : IDto
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
