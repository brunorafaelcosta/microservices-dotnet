using System.Collections.Generic;

namespace Transversal.Application.Dto.Response
{
    public class PaginatedResponseDto<TDto> : PaginatedResponseDtoBase<TDto>,
        IPaginatedResponse<TDto>
        where TDto : IDto
    {
        public PaginatedResponseDto(IList<TDto> result)
            : base(result)
        {
        }
    }
}
