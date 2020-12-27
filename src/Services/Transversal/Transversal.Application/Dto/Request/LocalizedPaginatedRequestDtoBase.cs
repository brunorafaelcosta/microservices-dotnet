namespace Transversal.Application.Dto.Request
{
    public abstract class LocalizedPaginatedRequestDtoBase<TResponseDto> : PaginatedRequestDtoBase<TResponseDto>,
        ILocalizedPaginatedRequestDto<TResponseDto>
        where TResponseDto : IDto
    {
        public virtual string LanguageCode { get; set; }
    }
}
