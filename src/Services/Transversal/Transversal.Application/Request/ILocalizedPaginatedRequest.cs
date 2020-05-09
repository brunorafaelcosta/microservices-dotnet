using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    /// <summary>
    /// List request parameters used by <see cref="IApplicationService"/> localized paginated methods.
    /// </summary>
    /// <typeparam name="TRequestDto">Data Transfer Object type used to pass query parameters</typeparam>
    /// <typeparam name="TResponseDto">Data Transfer Object response type</typeparam>
    public interface ILocalizedPaginatedRequest<TRequestDto, TResponseDto> : IPaginatedRequest<TRequestDto, TResponseDto>,
        ILocalizedListRequest<TRequestDto, TResponseDto>
        where TRequestDto : IDto
        where TResponseDto : IDto
    {
    }
}
