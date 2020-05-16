namespace Transversal.Application.Dto.Request
{
    public abstract class BaseLocalizedRequestDto : BaseRequestDto,
        ILocalizedRequestDto
    {
        public virtual string LanguageCode { get; set; }
    }
}
