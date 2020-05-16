namespace Transversal.Application.Dto.Request
{
    public abstract class BaseLocalizedListRequestDto<TResponseDto> : BaseListRequestDto<TResponseDto>,
        ILocalizedListRequestDto<TResponseDto>
        where TResponseDto : IDto
    {
        public virtual string LanguageCode { get; set; }
    }
}
