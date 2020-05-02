using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    /// <summary>
    /// List request parameters used by <see cref="IApplicationService"/> localized methods.
    /// </summary>
    /// <typeparam name="TDto">Data Transfer Object type used to pass query parameters</typeparam>
    public interface ILocalizedListRequest<TDto> : IListRequest<TDto>
        where TDto : IDto
    {
        /// <summary>
        /// Entities language
        /// </summary>
        string Language { get; set; }
    }
}
