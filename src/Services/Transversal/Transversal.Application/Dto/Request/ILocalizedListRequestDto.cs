namespace Transversal.Application.Dto.Request
{
    /// <summary>
    /// Localized list request Data Transfer Object used by <see cref="IApplicationService"/> methods.
    /// </summary>
    /// <typeparam name="TResponseDto">Data Transfer Object response type</typeparam>
    public interface ILocalizedListRequestDto<TResponseDto> : ILocalizedRequestDto,
        IListRequestDto<TResponseDto>
        where TResponseDto : IDto
    {
    }
}
