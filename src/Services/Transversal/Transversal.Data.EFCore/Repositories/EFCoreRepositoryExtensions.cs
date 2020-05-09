using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Transversal.Data.EFCore.Repositories
{
    /// <summary>
    /// Extension methods for <see cref="EFCoreRepositoryBase{TDbContext, TEntity, TPrimaryKey}"/> objects.
    /// </summary>
    public static class EFCoreRepositoryExtensions
    {
        public static IQueryable<T> ApplyPredicate<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            return queryable.AsQueryable();
        }

        public static IQueryable<T> ApplySort<T>(this IQueryable<T> queryable, Dictionary<Expression<Func<T, object>>, ListSortDirection> sort)
        {
            if (sort != null)
            {
                IOrderedQueryable<T> orderedQueryable = null;

                foreach (var sortProperty in sort)
                {
                    if (sortProperty.Value == ListSortDirection.Ascending)
                    {
                        orderedQueryable = orderedQueryable is null
                            ? queryable.OrderBy(sortProperty.Key)
                            : orderedQueryable.ThenBy(sortProperty.Key);
                    }
                    else
                    {
                        orderedQueryable = orderedQueryable is null
                            ? queryable.OrderByDescending(sortProperty.Key)
                            : orderedQueryable.ThenByDescending(sortProperty.Key);
                    }
                }

                queryable = orderedQueryable is null
                    ? queryable
                    : orderedQueryable.AsQueryable();
            }

            return queryable.AsQueryable();
        }

        public static IQueryable<T> ApplySubSequence<T>(this IQueryable<T> queryable, int? entitiesToSkip, int? entitiesToTake)
        {
            if (entitiesToSkip.GetValueOrDefault() >= 0 && entitiesToTake.GetValueOrDefault() > 0)
            {
                queryable = queryable.Skip(entitiesToSkip.Value).Take(entitiesToTake.Value);
            }

            return queryable.AsQueryable();
        }

        public static IQueryable<TProjection> ApplyProjection<TSource, TProjection>(this IQueryable<TSource> queryable,
            Expression<Func<TSource, TProjection>> projection)
        {
            return queryable.Select(projection).AsQueryable();
        }
    }
}
