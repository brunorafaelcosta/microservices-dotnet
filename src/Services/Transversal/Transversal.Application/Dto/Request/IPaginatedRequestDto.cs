namespace Transversal.Application.Dto.Request
{
    /// <summary>
    /// Paginated request Data Transfer Object used by <see cref="IApplicationService"/> methods.
    /// </summary>
    /// <typeparam name="TResponseDto">Data Transfer Object response type</typeparam>
    public interface IPaginatedRequestDto<TResponseDto> : IListRequestDto<TResponseDto>
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
