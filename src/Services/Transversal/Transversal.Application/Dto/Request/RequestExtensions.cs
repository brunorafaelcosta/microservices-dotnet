using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Transversal.Application.Exceptions;
using Transversal.Domain.Entities;
using Transversal.Domain.Repositories.Options;

namespace Transversal.Application.Dto.Request
{
    /// <summary>
    /// Extension methods for Request Data Transfer Objects.
    /// </summary>
    public static class RequestExtensions
    {
        #region Repository Options
        public static GetAllOptions<TEntity, int> ResolveRepositoryGetAllOptions<TEntity, TResponseDto>(
            this IPaginatedRequestDto<TResponseDto> request)
            where TEntity : class, IEntity
            where TResponseDto : class, IDto
        {
            return ResolveRepositoryGetAllOptions<TEntity, int, TResponseDto>(request);
        }

        public static GetAllOptions<TEntity, TEntityPrimaryKey> ResolveRepositoryGetAllOptions<TEntity, TEntityPrimaryKey, TResponseDto>(
            this IPaginatedRequestDto<TResponseDto> request)
            where TEntity : class, IEntity<TEntityPrimaryKey>
            where TResponseDto : class, IDto
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var options = new GetAllOptions<TEntity, TEntityPrimaryKey>
            {
                EntitiesToSkip = (request.PageIndex - 1) * request.PageSize,
                EntitiesToTake = request.PageSize
            };

            return options;
        }

        public static GetAllProjectedOptions<TEntity, int, TResponseDto> ResolveRepositoryGetAllProjectedOptions<TEntity, TResponseDto>(
            this ILocalizedPaginatedRequestDto<TResponseDto> request,
            Expression<Func<TEntity, TResponseDto>> projection)
            where TEntity : class, IEntity
            where TResponseDto : class, IDto
        {
            return ResolveRepositoryGetAllProjectedOptions<TEntity, int, TResponseDto>(request, projection);
        }

        public static GetAllProjectedOptions<TEntity, TEntityPrimaryKey, TResponseDto> ResolveRepositoryGetAllProjectedOptions<TEntity, TEntityPrimaryKey, TResponseDto>(
            this ILocalizedPaginatedRequestDto<TResponseDto> request,
            Expression<Func<TEntity, TResponseDto>> projection)
            where TEntity : class, IEntity<TEntityPrimaryKey>
            where TResponseDto : class, IDto
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));
            if (projection is null)
                throw new ArgumentNullException(nameof(projection));

            var options = new GetAllProjectedOptions<TEntity, TEntityPrimaryKey, TResponseDto>
            {
                EntitiesToSkip = (request.PageIndex - 1) * request.PageSize,
                EntitiesToTake = request.PageSize,
                Projection = projection,
                Sort = request?.Sort
            };

            return options;
        }
        #endregion Repository Options

        public static void SetRequestSortExpressions<TResponseDto>(
            this IListRequestDto<TResponseDto> request,
            Dictionary<string, string> sortPropertiesList)
            where TResponseDto : class, IDto
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            request.Sort = null;

            if (sortPropertiesList != null)
            {
                request.Sort = new Dictionary<Expression<Func<TResponseDto, object>>, ListSortDirection>();

                foreach (var sortProperty in sortPropertiesList)
                {
                    var sortPropertyName = sortProperty.Key;
                    var sortPropertyDirectionStr = sortProperty.Value;

                    if (string.IsNullOrEmpty(sortPropertyName))
                        throw new AppException($"Invalid list request sort property name[{sortPropertyName}]");
                    if (string.IsNullOrEmpty(sortPropertyDirectionStr))
                        throw new AppException($"Invalid list request sort direction [{sortPropertyDirectionStr}]");

                    var sortPropertyDirection = sortPropertyDirectionStr.ToListSortDirection();

                    Type currentType = typeof(TResponseDto);
                    ParameterExpression parameter = Expression.Parameter(currentType, "dto");
                    Expression expression = parameter;

                    int i = 0;
                    List<string> propertyChain = sortPropertyName.Split('.').ToList();
                    do
                    {
                        System.Reflection.PropertyInfo propertyInfo = currentType.GetProperty(propertyChain[i]);
                        currentType = propertyInfo.PropertyType;
                        i++;
                        if (propertyChain.Count == i)
                        {
                            currentType = typeof(object);
                        }
                        expression = Expression.Convert(Expression.PropertyOrField(expression, propertyInfo.Name), currentType);
                    } while (propertyChain.Count > i);

                    request.Sort.Add(Expression.Lambda<Func<TResponseDto, dynamic>>(expression, parameter), sortPropertyDirection);
                }
            }
        }

        public static ListSortDirection ToListSortDirection(this string listRequestSortDirection)
        {
            if (string.IsNullOrEmpty(listRequestSortDirection))
                throw new ArgumentNullException(nameof(listRequestSortDirection));

            switch (listRequestSortDirection.ToLowerInvariant())
            {
                case ListRequestDtoSortDirection.Ascending:
                    return ListSortDirection.Ascending;
                case ListRequestDtoSortDirection.Descending:
                    return ListSortDirection.Descending;
                default:
                    throw new AppException($"Invalid list request sort direction [{listRequestSortDirection}]");
            }
        }
    }
}
