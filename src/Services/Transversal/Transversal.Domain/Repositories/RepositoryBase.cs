using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Transversal.Domain.Entities;
using Transversal.Domain.Exceptions;

namespace Transversal.Domain.Repositories
{
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected abstract IQueryable<TEntity> AsQueryable();

        #region Select/Get/Query

        /// <inheritdoc/>
        public virtual List<TEntity> GetAllList()
        {
            return AsQueryable().ToList();
        }

        /// <inheritdoc/>
        public virtual Task<List<TEntity>> GetAllListAsync()
        {
            return Task.FromResult(GetAllList());
        }

        /// <inheritdoc/>
        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return AsQueryable().Where(predicate).ToList();
        }

        /// <inheritdoc/>
        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(GetAllList(predicate));
        }

        /// <inheritdoc/>
        public virtual TEntity Get(TPrimaryKey id)
        {
            var entity = FirstOrDefault(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var entity = await FirstOrDefaultAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        /// <inheritdoc/>
        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return AsQueryable().Single(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Single(predicate));
        }

        /// <inheritdoc/>
        public virtual TEntity FirstOrDefault(TPrimaryKey id)
        {
            return AsQueryable().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return Task.FromResult(FirstOrDefault(id));
        }

        /// <inheritdoc/>
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return AsQueryable().FirstOrDefault(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }

        #endregion Select/Get/Query

        #region Insert

        /// <inheritdoc/>
        public abstract TEntity Insert(TEntity entity);

        /// <inheritdoc/>
        public virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        /// <inheritdoc/>
        public virtual TPrimaryKey InsertAndGetId(TEntity entity)
        {
            return Insert(entity).Id;
        }

        /// <inheritdoc/>
        public virtual async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            var insertedEntity = await InsertAsync(entity);
            return insertedEntity.Id;
        }

        /// <inheritdoc/>
        public virtual TEntity InsertOrUpdate(TEntity entity)
        {
            return entity.IsTransient()
                ? Insert(entity)
                : Update(entity);
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            return entity.IsTransient()
                ? await InsertAsync(entity)
                : await UpdateAsync(entity);
        }

        /// <inheritdoc/>
        public virtual TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            return InsertOrUpdate(entity).Id;
        }

        /// <inheritdoc/>
        public virtual async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            var insertedEntity = await InsertOrUpdateAsync(entity);
            return insertedEntity.Id;
        }

        #endregion Insert

        #region Update

        /// <inheritdoc/>
        public abstract TEntity Update(TEntity entity);

        /// <inheritdoc/>
        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        #endregion Update

        #region Delete

        /// <inheritdoc/>
        public abstract void Delete(TEntity entity);

        /// <inheritdoc/>
        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public abstract void Delete(TPrimaryKey id);

        /// <inheritdoc/>
        public virtual Task DeleteAsync(TPrimaryKey id)
        {
            Delete(id);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = GetAllList(predicate);

            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        /// <inheritdoc/>
        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await GetAllListAsync(predicate);

            foreach (var entity in entities)
            {
                await DeleteAsync(entity);
            }
        }

        #endregion Delete

        #region Aggregates

        /// <inheritdoc/>
        public virtual long Count()
        {
            return AsQueryable().Count();
        }

        /// <inheritdoc/>
        public virtual Task<long> CountAsync()
        {
            return Task.FromResult(Count());
        }

        /// <inheritdoc/>
        public virtual long Count(Expression<Func<TEntity, bool>> predicate)
        {
            return AsQueryable().Count(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Count(predicate));
        }

        /// <inheritdoc/>
        public virtual bool Exists(TPrimaryKey id)
        {
            return AsQueryable().Any(CreateEqualityExpressionForId(id));
        }

        /// <inheritdoc/>
        public virtual Task<bool> ExistsAsync(TPrimaryKey id)
        {
            return Task.FromResult(Exists(id));
        }

        /// <inheritdoc/>
        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return AsQueryable().Any(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Exists(predicate));
        }

        #endregion Aggregates
        
        protected virtual Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var leftExpression = Expression.PropertyOrField(lambdaParam, nameof(IEntity.Id));

            var idValue = Convert.ChangeType(id, typeof(TPrimaryKey));

            Expression<Func<object>> closure = () => idValue;
            var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);

            var lambdaBody = Expression.Equal(leftExpression, rightExpression);

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }

    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity
    {
    }
}
