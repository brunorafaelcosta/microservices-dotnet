using System.Collections.Generic;

namespace Transversal.Application.Dto.Response
{
    public class ListResponseDto<TDto> : ListResponseDtoBase<TDto>,
        IListResponseDto<TDto>
        where TDto : IDto
    {
        public ListResponseDto(IList<TDto> result)
            : base(result)
        {
        }
    }
}
