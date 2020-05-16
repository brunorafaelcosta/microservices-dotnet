namespace Transversal.Application.Dto.Request
{
    /// <summary>
    /// Localized paginated request Data Transfer Object used by <see cref="IApplicationService"/> methods.
    /// </summary>
    /// <typeparam name="TResponseDto">Data Transfer Object response type</typeparam>
    public interface ILocalizedPaginatedRequestDto<TResponseDto> : ILocalizedRequestDto,
        IPaginatedRequestDto<TResponseDto>
        where TResponseDto : IDto
    {
    }
}
