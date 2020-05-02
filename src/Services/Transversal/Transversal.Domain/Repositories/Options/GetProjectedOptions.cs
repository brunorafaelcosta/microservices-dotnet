using System;
using System.Linq.Expressions;
using Transversal.Domain.Entities;

namespace Transversal.Domain.Repositories.Options
{
    /// <summary>
    /// Query options for <see cref="IRepository{TEntity, TPrimaryKey}"/> Get projected methods.
    /// </summary>
    /// <typeparam name="TEntity">Repository entity type</typeparam>
    /// <typeparam name="TEntityPrimaryKey">Primary key type of the entity</typeparam>
    /// <typeparam name="TProjection">>Projection type</typeparam>
    public class GetProjectedOptions<TEntity, TEntityPrimaryKey, TProjection> : GetOptions<TEntity, TEntityPrimaryKey>
        where TEntity : class, IEntity<TEntityPrimaryKey>
        where TProjection : class
    {
        /// <summary>
        /// Projection expression
        /// </summary>
        public Expression<Func<TEntity, TProjection>> Projection { get; set; }
    }
}
