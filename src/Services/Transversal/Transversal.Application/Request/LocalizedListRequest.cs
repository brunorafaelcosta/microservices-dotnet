using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    public class LocalizedListRequest<TDto> : ListRequest<TDto>,
        ILocalizedListRequest<TDto>
        where TDto : IDto
    {
        public string Language { get; set; }

        public LocalizedListRequest()
            : base()
        {
        }
    }
}
