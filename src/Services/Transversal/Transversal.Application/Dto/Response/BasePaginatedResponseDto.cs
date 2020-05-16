using System.Collections.Generic;

namespace Transversal.Application.Dto.Response
{
    public abstract class BasePaginatedResponseDto<TDto> : BaseListResponseDto<TDto>,
        IPaginatedResponse<TDto>
        where TDto : IDto
    {
        public virtual int PageIndex { get; set; }

        public virtual int PageSize { get; set; }

        public virtual long Total { get; set; }

        public BasePaginatedResponseDto(IList<TDto> result)
            : base(result)
        {
        }
    }
}
