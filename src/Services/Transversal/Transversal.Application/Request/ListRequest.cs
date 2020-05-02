using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    public class ListRequest<TDto> : IListRequest<TDto>
        where TDto : IDto
    {
        public IDictionary<Expression<Func<TDto, object>>, ListSortDirection> Sort { get; set; }

        public ListRequest()
        {
            Sort = new Dictionary<Expression<Func<TDto, object>>, ListSortDirection>();
        }
    }
}
