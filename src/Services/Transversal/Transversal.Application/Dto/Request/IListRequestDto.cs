using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Transversal.Application.Dto.Request
{
    /// <summary>
    /// List request Data Transfer Object used by <see cref="IApplicationService"/> methods.
    /// </summary>
    /// <typeparam name="TResponseDto">Data Transfer Object response type</typeparam>
    public interface IListRequestDto<TResponseDto>
        where TResponseDto : IDto
    {
        /// <summary>
        /// Sort direction
        /// </summary>
        Dictionary<Expression<Func<TResponseDto, object>>, ListSortDirection> Sort { get; set; }
    }
}
