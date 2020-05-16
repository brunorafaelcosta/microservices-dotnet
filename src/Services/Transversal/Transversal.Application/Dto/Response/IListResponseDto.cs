using System.Collections.Generic;

namespace Transversal.Application.Dto.Response
{
    /// <summary>
    /// List response Data Transfer Object used by <see cref="IApplicationService"/> methods.
    /// </summary>
    /// <typeparam name="TDto">Data Transfer Object type used to return the mapped entities</typeparam>
    public interface IListResponseDto<TDto> : IResponseDto, IReadOnlyList<TDto>
        where TDto : IDto
    {
    }
}
