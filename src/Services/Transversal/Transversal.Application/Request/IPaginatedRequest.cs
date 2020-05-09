using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    /// <summary>
    /// List request parameters used by <see cref="IApplicationService"/> paginated methods.
    /// </summary>
    /// <typeparam name="TRequestDto">Data Transfer Object type used to pass query parameters</typeparam>
    /// <typeparam name="TResponseDto">Data Transfer Object response type</typeparam>
    public interface IPaginatedRequest<TRequestDto, TResponseDto> : IListRequest<TRequestDto, TResponseDto>
        where TRequestDto : IDto
        where TResponseDto : IDto
    {
        /// <summary>
        /// Page index to be fetched
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// Page size to limit the fetched entities
        /// </summary>
        int PageSize { get; set; }
    }
}
