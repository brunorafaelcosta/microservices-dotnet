namespace Transversal.Application.Dto.Request
{
    public abstract class LocalizedListRequestDtoBase<TResponseDto> : ListRequestDtoBase<TResponseDto>,
        ILocalizedListRequestDto<TResponseDto>
        where TResponseDto : IDto
    {
        public virtual string LanguageCode { get; set; }
    }
}
