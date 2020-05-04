using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Transversal.Domain.Entities;

namespace Transversal.Domain.Repositories.Options
{
    /// <summary>
    /// Query options for <see cref="IRepository{TEntity, TPrimaryKey}"/> GetAll methods.
    /// </summary>
    /// <typeparam name="TEntity">Repository entity type</typeparam>
    /// <typeparam name="TEntityPrimaryKey">Primary key type of the entity</typeparam>
    public class GetAllOptions<TEntity, TEntityPrimaryKey> : DefaultOptions<TEntity, TEntityPrimaryKey>
        where TEntity : class, IEntity<TEntityPrimaryKey>
    {
        /// <summary>
        /// Number of entities to skip
        /// </summary>
        public int? EntitiesToSkip { get; set; }

        /// <summary>
        /// Number of entities to fetch
        /// </summary>
        public int? EntitiesToTake { get; set; }

        /// <summary>
        /// Sort direction
        /// </summary>
        public virtual Dictionary<Expression<Func<TEntity, object>>, ListSortDirection> Sort { get; set; }
    }
}
