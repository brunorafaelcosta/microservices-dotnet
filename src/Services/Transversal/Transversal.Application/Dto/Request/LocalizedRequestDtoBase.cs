namespace Transversal.Application.Dto.Request
{
    public abstract class LocalizedRequestDtoBase : RequestDtoBase,
        ILocalizedRequestDto
    {
        public virtual string LanguageCode { get; set; }
    }
}
