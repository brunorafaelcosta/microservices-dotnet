using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Transversal.Domain.Entities;

namespace Transversal.Domain.Repositories.Options
{
    /// <summary>
    /// Query options for <see cref="IRepository{TEntity, TPrimaryKey}"/> GetAll projected methods.
    /// </summary>
    /// <typeparam name="TEntity">Repository entity type</typeparam>
    /// <typeparam name="TEntityPrimaryKey">Primary key type of the entity</typeparam>
    /// <typeparam name="TProjection">Projection type</typeparam>
    public class GetAllProjectedOptions<TEntity, TEntityPrimaryKey, TProjection> : GetAllOptions<TEntity, TEntityPrimaryKey>
        where TEntity : class, IEntity<TEntityPrimaryKey>
        where TProjection : class
    {
        /// <summary>
        /// Sort direction
        /// </summary>
        public new Dictionary<Expression<Func<TProjection, object>>, ListSortDirection> Sort { get; set; }

        /// <summary>
        /// Projection expression
        /// </summary>
        public Expression<Func<TEntity, TProjection>> Projection { get; set; }
    }
}
