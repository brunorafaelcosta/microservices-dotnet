using System.Collections.Generic;
using Transversal.Application.Dto;

namespace Transversal.Application.Response
{
    public class PaginatedResponse<TDto> : ListResponse<TDto>,
        IPaginatedResponse<TDto>
        where TDto : IDto
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long Total { get; set; }

        public PaginatedResponse()
            : base()
        {
        }

        public PaginatedResponse(IReadOnlyList<TDto> result)
            : base(result)
        {
        }
    }
}
