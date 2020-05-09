using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    /// <summary>
    /// List request parameters used by <see cref="IApplicationService"/> localized methods.
    /// </summary>
    /// <typeparam name="TRequestDto">Data Transfer Object type used to pass query parameters</typeparam>
    /// <typeparam name="TResponseDto">Data Transfer Object response type</typeparam>
    public interface ILocalizedListRequest<TRequestDto, TResponseDto> : IListRequest<TRequestDto, TResponseDto>
        where TRequestDto : IDto
        where TResponseDto : IDto
    {
        /// <summary>
        /// Entities overridden language
        /// </summary>
        string OverriddenLanguage { get; set; }
    }
}
