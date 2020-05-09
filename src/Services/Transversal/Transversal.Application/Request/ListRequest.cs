using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Transversal.Application.Dto;

namespace Transversal.Application.Request
{
    public class ListRequest<TRequestDto, TResponseDto> : IListRequest<TRequestDto, TResponseDto>
        where TRequestDto : IDto
        where TResponseDto : IDto
    {
        public Dictionary<Expression<Func<TResponseDto, object>>, ListSortDirection> Sort { get; set; }

        public ListRequest()
        {
            Sort = new Dictionary<Expression<Func<TResponseDto, object>>, ListSortDirection>();
        }
    }
}
