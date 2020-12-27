using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Transversal.Application.Dto.Response
{
    public abstract class ListResponseDtoBase<TDto> : ReadOnlyCollection<TDto>,
        IListResponseDto<TDto>
        where TDto : IDto
    {
        public ListResponseDtoBase(IList<TDto> list)
            : base(list)
        {
        }
    }
}
