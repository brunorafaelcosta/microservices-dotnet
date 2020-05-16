using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Transversal.Application.Dto.Response
{
    public abstract class BaseListResponseDto<TDto> : ReadOnlyCollection<TDto>,
        IListResponseDto<TDto>
        where TDto : IDto
    {
        public BaseListResponseDto(IList<TDto> list) : base(list)
        {
        }
    }
}
