using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        protected abstract IQueryable<TEntity> AsQueryable(string overriddenLanguage = null);
        protected abstract IQueryable<TEntity> AsQueryable(
            Expression<Func<TEntity, bool>> predicate = null,
            Dictionary<Expression<Func<TEntity, object>>, ListSortDirection> sort = null,
            int? entitiesToSkip = null, int? entitiesToTake = null,
            string overriddenLanguage = null);

        protected abstract IQueryable<TProjection> AsQueryable<TProjection>(
            Expression<Func<TEntity, TProjection>> projection) where TProjection : class;
        protected abstract IQueryable<TProjection> AsQueryable<TProjection>(
            Expression<Func<TEntity, TProjection>> projection,
            string overriddenLanguage = null) where TProjection : class;
        protected abstract IQueryable<TProjection> AsQueryable<TProjection>(
            Expression<Func<TEntity, TProjection>> projection,
            Expression<Func<TEntity, bool>> predicate = null,
            Dictionary<Expression<Func<TProjection, object>>, ListSortDirection> sort = null,
            int? entitiesToSkip = null, int? entitiesToTake = null,
            string overriddenLanguage = null) where TProjection : class;

        #region Select/Get/Query

        public virtual List<TEntity> GetAllList(Options.GetAllOptions<TEntity, TPrimaryKey> options)
        {
            IQueryable<TEntity> query = AsQueryable(
                predicate: null,
                sort : options?.Sort,
                entitiesToSkip: options?.EntitiesToSkip,
                entitiesToTake: options?.EntitiesToTake,
                overriddenLanguage: options?.OverriddenLanguage);

            return query.ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync(Options.GetAllOptions<TEntity, TPrimaryKey> options)
        {
            return Task.FromResult(GetAllList(options));
        }

        public virtual List<TEntity> GetAllList(Options.GetAllOptions<TEntity, TPrimaryKey> options, out long totalEntities)
        {
            IQueryable<TEntity> countQuery = AsQueryable(overriddenLanguage: options?.OverriddenLanguage);

            totalEntities = countQuery.LongCount();

            IQueryable<TEntity> listQuery = AsQueryable(
                predicate: null,
                sort: options?.Sort,
                entitiesToSkip: options?.EntitiesToSkip,
                entitiesToTake: options?.EntitiesToTake,
                overriddenLanguage: options?.OverriddenLanguage);

            return listQuery.ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync(Options.GetAllOptions<TEntity, TPrimaryKey> options, out long totalEntities)
        {
            return Task.FromResult(GetAllList(options, out totalEntities));
        }

        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate, Options.GetAllOptions<TEntity, TPrimaryKey> options)
        {
            IQueryable<TEntity> query = AsQueryable(
                predicate: predicate,
                sort: options?.Sort,
                entitiesToSkip: options?.EntitiesToSkip,
                entitiesToTake: options?.EntitiesToTake,
                overriddenLanguage: options?.OverriddenLanguage);

            return query.ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate, Options.GetAllOptions<TEntity, TPrimaryKey> options)
        {
            return Task.FromResult(GetAllList(predicate, options));
        }

        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate, Options.GetAllOptions<TEntity, TPrimaryKey> options, out long totalEntities)
        {
            IQueryable<TEntity> countQuery = AsQueryable(
                predicate: predicate,
                overriddenLanguage: options?.OverriddenLanguage);

            totalEntities = countQuery.LongCount();

            IQueryable<TEntity> listQuery = AsQueryable(
                predicate: predicate,
                sort: options?.Sort,
                entitiesToSkip: options?.EntitiesToSkip,
                entitiesToTake: options?.EntitiesToTake,
                overriddenLanguage: options?.OverriddenLanguage);

            return listQuery.ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate, Options.GetAllOptions<TEntity, TPrimaryKey> options, out long totalEntities)
        {
            return Task.FromResult(GetAllList(predicate, options, out totalEntities));
        }

        public virtual List<TProjection> GetAllList<TProjection>(Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class
        {
            IQueryable<TProjection> query = AsQueryable<TProjection>(
                projection: options?.Projection,
                predicate: null,
                sort: options?.Sort,
                entitiesToSkip: options?.EntitiesToSkip,
                entitiesToTake: options?.EntitiesToTake,
                overriddenLanguage: options?.OverriddenLanguage);

            return query.ToList();
        }

        public virtual Task<List<TProjection>> GetAllListAsync<TProjection>(Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class
        {
            return Task.FromResult(GetAllList<TProjection>(options));
        }

        public virtual List<TProjection> GetAllList<TProjection>(Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options, out long totalEntities) where TProjection : class
        {
            IQueryable<TProjection> countQuery = AsQueryable<TProjection>(
                projection: options?.Projection,
                overriddenLanguage: options?.OverriddenLanguage);

            totalEntities = countQuery.LongCount();

            IQueryable<TProjection> listQuery = AsQueryable<TProjection>(
                projection: options?.Projection,
                predicate: null,
                sort: options?.Sort,
                entitiesToSkip: options?.EntitiesToSkip,
                entitiesToTake: options?.EntitiesToTake,
                overriddenLanguage: options?.OverriddenLanguage);

            return listQuery.ToList();
        }

        public virtual Task<List<TProjection>> GetAllListAsync<TProjection>(Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options, out long totalEntities) where TProjection : class
        {
            return Task.FromResult(GetAllList<TProjection>(options, out totalEntities));
        }

        public virtual List<TProjection> GetAllList<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class
        {
            IQueryable<TProjection> query = AsQueryable<TProjection>(
                projection: options?.Projection,
                predicate: predicate,
                sort: options?.Sort,
                entitiesToSkip: options?.EntitiesToSkip,
                entitiesToTake: options?.EntitiesToTake,
                overriddenLanguage: options?.OverriddenLanguage);

            return query.ToList();
        }

        public virtual Task<List<TProjection>> GetAllListAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class
        {
            return Task.FromResult(GetAllList<TProjection>(predicate, options));
        }

        public virtual List<TProjection> GetAllList<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options, out long totalEntities) where TProjection : class
        {
            IQueryable<TProjection> countQuery = AsQueryable<TProjection>(
                projection: options?.Projection,
                predicate: predicate,
                overriddenLanguage: options?.OverriddenLanguage);

            totalEntities = countQuery.LongCount();

            IQueryable<TProjection> listQuery = AsQueryable<TProjection>(
                projection: options?.Projection,
                predicate: predicate,
                sort: options?.Sort,
                entitiesToSkip: options?.EntitiesToSkip,
                entitiesToTake: options?.EntitiesToTake,
                overriddenLanguage: options?.OverriddenLanguage);

            return listQuery.ToList();
        }

        public virtual Task<List<TProjection>> GetAllListAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options, out long totalEntities) where TProjection : class
        {
            return Task.FromResult(GetAllList<TProjection>(predicate, options, out totalEntities));
        }

        public virtual TEntity Get(TPrimaryKey id, Options.GetOptions<TEntity, TPrimaryKey> options)
        {
            var entity = FirstOrDefault(id, options);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual async Task<TEntity> GetAsync(TPrimaryKey id, Options.GetOptions<TEntity, TPrimaryKey> options)
        {
            var entity = await FirstOrDefaultAsync(id, options);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public TProjection Get<TProjection>(TPrimaryKey id, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class
        {
            var projection = FirstOrDefault<TProjection>(id, options);
            if (projection == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return projection;
        }

        public virtual async Task<TProjection> GetAsync<TProjection>(TPrimaryKey id, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class
        {
            var projection = await FirstOrDefaultAsync<TProjection>(id, options);
            if (projection == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return projection;
        }

        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate, Options.GetOptions<TEntity, TPrimaryKey> options)
        {
            IQueryable<TEntity> query = AsQueryable(predicate: predicate);

            return query.Single();
        }

        public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, Options.GetOptions<TEntity, TPrimaryKey> options)
        {
            return Task.FromResult(Single(predicate, options));
        }

        public TProjection Single<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class
        {
            IQueryable<TProjection> query = AsQueryable<TProjection>(
                projection: options?.Projection,
                predicate: predicate,
                overriddenLanguage: options?.OverriddenLanguage);

            return query.Single();
        }

        public virtual Task<TProjection> SingleAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class
        {
            return Task.FromResult(Single<TProjection>(predicate, options));
        }

        public virtual TEntity FirstOrDefault(TPrimaryKey id, Options.GetOptions<TEntity, TPrimaryKey> options)
        {
            IQueryable<TEntity> query = AsQueryable(
                predicate: CreateEqualityExpressionForId(id),
                overriddenLanguage: options?.OverriddenLanguage);

            return query.FirstOrDefault();
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id, Options.GetOptions<TEntity, TPrimaryKey> options)
        {
            return Task.FromResult(FirstOrDefault(id, options));
        }

        public virtual TProjection FirstOrDefault<TProjection>(TPrimaryKey id, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class
        {
            IQueryable<TProjection> query = AsQueryable<TProjection>(
                projection: options?.Projection,
                predicate: CreateEqualityExpressionForId(id),
                overriddenLanguage: options?.OverriddenLanguage);

            return query.FirstOrDefault();
        }

        public virtual Task<TProjection> FirstOrDefaultAsync<TProjection>(TPrimaryKey id, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class
        {
            return Task.FromResult(FirstOrDefault<TProjection>(id, options));
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, Options.GetOptions<TEntity, TPrimaryKey> options)
        {
            IQueryable<TEntity> query = AsQueryable(
                predicate: predicate,
                overriddenLanguage: options?.OverriddenLanguage);

            return query.FirstOrDefault();
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, Options.GetOptions<TEntity, TPrimaryKey> options)
        {
            return Task.FromResult(FirstOrDefault(predicate, options));
        }

        public virtual TProjection FirstOrDefault<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class
        {
            IQueryable<TProjection> query = AsQueryable<TProjection>(
                projection: options?.Projection,
                predicate: predicate,
                overriddenLanguage: options?.OverriddenLanguage);

            return query.FirstOrDefault();
        }

        public virtual Task<TProjection> FirstOrDefaultAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class
        {
            return Task.FromResult(FirstOrDefault<TProjection>(predicate, options));
        }

        #endregion Select/Get/Query

        #region Insert

        public abstract TEntity Insert(TEntity entity);

        public virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public virtual TPrimaryKey InsertAndGetId(TEntity entity)
        {
            return Insert(entity).Id;
        }

        public virtual async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            var insertedEntity = await InsertAsync(entity);
            return insertedEntity.Id;
        }

        public virtual TEntity InsertOrUpdate(TEntity entity)
        {
            return entity.IsTransient()
                ? Insert(entity)
                : Update(entity);
        }

        public virtual async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            return entity.IsTransient()
                ? await InsertAsync(entity)
                : await UpdateAsync(entity);
        }

        public virtual TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            return InsertOrUpdate(entity).Id;
        }

        public virtual async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            var insertedEntity = await InsertOrUpdateAsync(entity);
            return insertedEntity.Id;
        }

        #endregion Insert

        #region Update

        public abstract TEntity Update(TEntity entity);

        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        #endregion Update

        #region Delete

        public abstract void Delete(TEntity entity);

        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.CompletedTask;
        }

        public abstract void Delete(TPrimaryKey id);

        public virtual Task DeleteAsync(TPrimaryKey id)
        {
            Delete(id);
            return Task.CompletedTask;
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = AsQueryable(predicate).ToList();

            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.Run(async () =>
            {
                var entities = AsQueryable(predicate).ToList();

                foreach (var entity in entities)
                {
                    await DeleteAsync(entity);
                }
            });
        }

        #endregion Delete

        #region Aggregates

        public virtual long Count()
        {
            return AsQueryable().Count();
        }

        public virtual Task<long> CountAsync()
        {
            return Task.FromResult(Count());
        }

        public virtual long Count(Expression<Func<TEntity, bool>> predicate)
        {
            return AsQueryable().Count(predicate);
        }

        public virtual Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Count(predicate));
        }

        public virtual bool Exists(TPrimaryKey id)
        {
            return AsQueryable().Any(CreateEqualityExpressionForId(id));
        }

        public virtual Task<bool> ExistsAsync(TPrimaryKey id)
        {
            return Task.FromResult(Exists(id));
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return AsQueryable().Any(predicate);
        }

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
