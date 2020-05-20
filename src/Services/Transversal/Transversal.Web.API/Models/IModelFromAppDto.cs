using Transversal.Application.Dto;

namespace Transversal.Web.API.Models
{
    /// <summary>
    /// API model that is mapped from an existing Application Data Transfer Object.
    /// </summary>
    /// <typeparam name="TDto">Data Transfer Object type</typeparam>
    public interface IModelFromAppDto<TDto> : IModel
        where TDto : IDto
    {
        /// <summary>
        /// Maps the <paramref name="dto"/> properties to the current object
        /// </summary>
        /// <param name="dto">Data Transfer Object</param>
        void FromAppDto(TDto dto);
    }
}
