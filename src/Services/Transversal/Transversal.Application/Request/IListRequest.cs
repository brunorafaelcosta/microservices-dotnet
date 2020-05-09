using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    /// <summary>
    /// List request parameters used by <see cref="IApplicationService"/> methods.
    /// </summary>
    /// <typeparam name="TRequestDto">Data Transfer Object type used to pass query parameters</typeparam>
    /// <typeparam name="TResponseDto">Data Transfer Object response type</typeparam>
    public interface IListRequest<TRequestDto, TResponseDto>
        where TRequestDto : IDto
        where TResponseDto : IDto
    {
        /// <summary>
        /// Sort direction
        /// </summary>
        Dictionary<Expression<Func<TResponseDto, object>>, ListSortDirection> Sort { get; set; }
    }
}
