using System.Collections.Generic;

namespace Transversal.Application.Dto.Response
{
    public abstract class PaginatedResponseDtoBase<TDto> : ListResponseDtoBase<TDto>,
        IPaginatedResponse<TDto>
        where TDto : IDto
    {
        public PaginatedResponseDtoBase(IList<TDto> result)
            : base(result)
        {
        }

        public virtual int PageIndex { get; set; }

        public virtual int PageSize { get; set; }

        public virtual long Total { get; set; }
    }
}
