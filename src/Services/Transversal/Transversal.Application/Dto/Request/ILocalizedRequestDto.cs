namespace Transversal.Application.Dto.Request
{
    /// <summary>
    /// Localized request Data Transfer Object used by <see cref="IApplicationService"/> methods.
    /// </summary>
    public interface ILocalizedRequestDto : IRequestDto
    {
        /// <summary>
        /// Entity language code
        /// </summary>
        string LanguageCode { get; set; }
    }
}
