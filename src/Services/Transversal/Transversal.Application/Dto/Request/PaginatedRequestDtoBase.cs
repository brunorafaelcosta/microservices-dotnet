namespace Transversal.Application.Dto.Request
{
    public abstract class PaginatedRequestDtoBase<TResponseDto> : ListRequestDtoBase<TResponseDto>,
        IPaginatedRequestDto<TResponseDto>
        where TResponseDto : IDto
    {
        public virtual int PageIndex { get; set; }

        public virtual int PageSize { get; set; }
    }
}
