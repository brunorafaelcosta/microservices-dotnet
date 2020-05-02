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
    /// <typeparam name="TDto">Data Transfer Object type used to pass query parameters</typeparam>
    public interface IListRequest<TDto>
        where TDto : IDto
    {
        /// <summary>
        /// Sort direction
        /// </summary>
        IDictionary<Expression<Func<TDto, object>>, ListSortDirection> Sort { get; set; }
    }
}
