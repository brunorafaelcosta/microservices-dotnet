using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    /// <summary>
    /// List request parameters used by <see cref="IApplicationService"/> paginated methods.
    /// </summary>
    /// <typeparam name="TDto">Data Transfer Object type used to pass query parameters</typeparam>
    public interface IPaginatedRequest<TDto> : IListRequest<TDto>
        where TDto : IDto
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
