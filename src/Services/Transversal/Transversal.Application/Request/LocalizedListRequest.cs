using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    public class LocalizedListRequest<TRequestDto, TResponseDto> : ListRequest<TRequestDto, TResponseDto>,
        ILocalizedListRequest<TRequestDto, TResponseDto>
        where TRequestDto : IDto
        where TResponseDto : IDto
    {
        public string LanguageCode { get; set; }

        public LocalizedListRequest()
            : base()
        {
        }
    }
}
