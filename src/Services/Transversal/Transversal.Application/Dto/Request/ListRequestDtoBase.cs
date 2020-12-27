using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Transversal.Application.Dto.Request
{
    public abstract class ListRequestDtoBase<TResponseDto> : RequestDtoBase,
        IListRequestDto<TResponseDto>
        where TResponseDto : IDto
    {
        public virtual Dictionary<Expression<Func<TResponseDto, object>>, ListSortDirection> Sort { get; set; }

        public ListRequestDtoBase()
        {
            Sort = new Dictionary<Expression<Func<TResponseDto, object>>, ListSortDirection>();
        }
    }
}
