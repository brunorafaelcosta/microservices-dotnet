using System.Collections.Generic;
using Transversal.Application.Dto;

namespace Transversal.Application.Response
{
    public class ListResponse<TDto> : Response<IReadOnlyList<TDto>>,
        IListResponse<TDto>
        where TDto : IDto
    {
        public ListResponse()
            : base()
        {
        }

        public ListResponse(IReadOnlyList<TDto> result)
            : base(result)
        {
        }
    }
}
