using System;
using System.Linq.Expressions;
using Transversal.Application.Dto;
using Transversal.Domain.Entities;
using Transversal.Domain.Repositories.Options;

namespace Transversal.Application.Request
{
    /// <summary>
    /// Extension methods for Request objects.
    /// </summary>
    public static class RequestExtensions
    {
        public static GetAllOptions<TEntity, int> ResolveRepositoryGetAllOptions<TEntity, TRequestDto, TResponseDto>(
            this IPaginatedRequest<TRequestDto> request)
            where TEntity : class, IEntity
            where TRequestDto : IDto
            where TResponseDto : class, IDto
        {
            return ResolveRepositoryGetAllOptions<TEntity, int, TRequestDto, TResponseDto>(request);
        }

        public static GetAllOptions<TEntity, TEntityPrimaryKey> ResolveRepositoryGetAllOptions<TEntity, TEntityPrimaryKey, TRequestDto, TResponseDto>(
            this IPaginatedRequest<TRequestDto> request)
            where TEntity : class, IEntity<TEntityPrimaryKey>
            where TRequestDto : IDto
            where TResponseDto : class, IDto
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var options = new GetAllOptions<TEntity, TEntityPrimaryKey>
            {
                EntitiesToTake = request.PageSize,
                EntitiesToSkip = (request.PageIndex - 1) * request.PageSize
            };

            return options;
        }

        public static GetAllProjectedOptions<TEntity, int, TResponseDto> ResolveRepositoryGetAllProjectedOptions<TEntity, TRequestDto, TResponseDto>(
            this ILocalizedPaginatedRequest<TRequestDto> request,
            Expression<Func<TEntity, TResponseDto>> projection)
            where TEntity : class, IEntity
            where TRequestDto : class, IDto
            where TResponseDto : class, IDto
        {
            return ResolveRepositoryGetAllProjectedOptions<TEntity, int, TRequestDto, TResponseDto>(request, projection);
        }

        public static GetAllProjectedOptions<TEntity, TEntityPrimaryKey, TResponseDto> ResolveRepositoryGetAllProjectedOptions<TEntity, TEntityPrimaryKey, TRequestDto, TResponseDto>(
            this ILocalizedPaginatedRequest<TRequestDto> request,
            Expression<Func<TEntity, TResponseDto>> projection)
            where TEntity : class, IEntity<TEntityPrimaryKey>
            where TRequestDto : class, IDto
            where TResponseDto : class, IDto
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));
            if (projection is null)
                throw new ArgumentNullException(nameof(projection));

            var options = new GetAllProjectedOptions<TEntity, TEntityPrimaryKey, TResponseDto>
            {
                EntitiesToTake = request.PageSize,
                EntitiesToSkip = (request.PageIndex - 1) * request.PageSize,
                Projection = projection
            };

            return options;
        }
    }
}
