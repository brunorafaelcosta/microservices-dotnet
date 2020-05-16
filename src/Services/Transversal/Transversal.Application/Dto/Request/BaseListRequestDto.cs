using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Transversal.Application.Dto.Request
{
    public abstract class BaseListRequestDto<TResponseDto> : BaseRequestDto,
        IListRequestDto<TResponseDto>
        where TResponseDto : IDto
    {
        public virtual Dictionary<Expression<Func<TResponseDto, object>>, ListSortDirection> Sort { get; set; }

        public BaseListRequestDto()
        {
            Sort = new Dictionary<Expression<Func<TResponseDto, object>>, ListSortDirection>();
        }
    }
}
