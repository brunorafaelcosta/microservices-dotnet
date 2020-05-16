namespace Transversal.Application.Dto.Request
{
    public class BaseLocalizedPaginatedRequestDto<TResponseDto> : BasePaginatedRequestDto<TResponseDto>,
        ILocalizedPaginatedRequestDto<TResponseDto>
        where TResponseDto : IDto
    {
        public virtual string LanguageCode { get; set; }
    }
}
