using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    /// <summary>
    /// List request parameters used by <see cref="IApplicationService"/> localized paginated methods.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public interface ILocalizedPaginatedRequest<TDto> : IPaginatedRequest<TDto>,
        ILocalizedListRequest<TDto>
        where TDto : IDto
    {
    }
}
