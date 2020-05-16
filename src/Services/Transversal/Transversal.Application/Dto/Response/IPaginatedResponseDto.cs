namespace Transversal.Application.Dto.Response
{
    /// <summary>
    /// Paginated response Data Transfer Object used by <see cref="IApplicationService"/> methods.
    /// </summary>
    /// <typeparam name="TDto">Data Transfer Object type used to return the mapped entities</typeparam>
    public interface IPaginatedResponse<TDto> : IListResponseDto<TDto>
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
