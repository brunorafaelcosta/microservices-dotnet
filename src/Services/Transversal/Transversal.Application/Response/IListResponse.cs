using System.Collections.Generic;
using Transversal.Application.Dto;

namespace Transversal.Application.Response
{
    /// <summary>
    /// List response parameters used by <see cref="IApplicationService"/> methods.
    /// </summary>
    /// <typeparam name="TDto">Data Transfer Object type used to return the mapped entities</typeparam>
    public interface IListResponse<TDto> : IResponse<IReadOnlyList<TDto>>
        where TDto : IDto
    {
    }
}
