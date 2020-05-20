using System;
using System.Collections.Generic;
using System.Linq;
using Transversal.Application.Dto;
using Transversal.Application.Dto.Response;

namespace Transversal.Web.API.Models.Response
{
    /// <summary>
    /// Extension methods for Response model object.
    /// </summary>
    public static class ResponseModelExtensions
    {
        public static TModel ToResponseModel<TModel, TDto>(
            this TDto dto)
            where TModel : class, IResponseModel, IModelFromAppDto<TDto>, new()
            where TDto : IDto
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            var model = new TModel();
            model.FromAppDto(dto);

            return model;
        }

        public static PaginatedResponseModel<TModel> ToPaginatedResponseModel<TModel, TDto>(
            this PaginatedResponseDto<TDto> paginatedResponseDtos)
            where TModel : class, IResponseModel, IModelFromAppDto<TDto>, new()
            where TDto : IDto
        {
            if (paginatedResponseDtos is null)
                throw new ArgumentNullException(nameof(paginatedResponseDtos));

            List<TModel> models = paginatedResponseDtos
                .Select(dto => dto.ToResponseModel<TModel, TDto>())
                .ToList();
            
            return new PaginatedResponseModel<TModel>(models)
            {
                PageIndex = paginatedResponseDtos.PageIndex,
                PageSize = paginatedResponseDtos.PageSize,
                Total = paginatedResponseDtos.Total
            };
        }
    }
}
