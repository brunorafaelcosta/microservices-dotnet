using Transversal.Application.Dto;

namespace Transversal.Application.Response
{
    /// <summary>
    /// List response parameters used by <see cref="IApplicationService"/> paginated methods.
    /// </summary>
    /// <typeparam name="TDto">Data Transfer Object type used to return the mapped entities</typeparam>
    public interface IPaginatedResponse<TDto> : IListResponse<TDto>
        where TDto : IDto
    {
        /// <summary>
        /// Current page
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// Used page size to limit the fetched entities
        /// </summary>
        int PageSize { get; }
        
        /// <summary>
        /// Total number of entities in the repository
        /// </summary>
        long Total { get; }
    }
}
