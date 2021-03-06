﻿using Transversal.Domain.Entities;

namespace Transversal.Domain.Repositories.Options
{
    /// <summary>
    /// Query options for <see cref="IRepository{TEntity, TPrimaryKey}"/> Get methods.
    /// </summary>
    /// <typeparam name="TEntity">Repository entity type</typeparam>
    /// <typeparam name="TEntityPrimaryKey">Primary key type of the entity</typeparam>
    public class GetOptions<TEntity, TEntityPrimaryKey> : DefaultOptions<TEntity, TEntityPrimaryKey>
        where TEntity : class, IEntity<TEntityPrimaryKey>
    {
    }
}
