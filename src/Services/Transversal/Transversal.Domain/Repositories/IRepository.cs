using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Transversal.Domain.Entities;

namespace Transversal.Domain.Repositories
{
    /// <summary>
    /// Interface implemented by all domain repositories.
    /// </summary>
    /// <typeparam name="TEntity">Entity type this repository works on</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public interface IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Select/Get/Query

        /// <summary>
        /// Used to get all entities.
        /// </summary>
        /// <param name="options">Query options</param>
        /// <returns>List of all entities</returns>
        List<TEntity> GetAllList(Options.GetAllOptions<TEntity, TPrimaryKey> options);

        /// <summary>
        /// Used to get all entities.
        /// </summary>
        /// <param name="options">Query options</param>
        /// <returns>List of all entities</returns>
        Task<List<TEntity>> GetAllListAsync(Options.GetAllOptions<TEntity, TPrimaryKey> options);

        /// <summary>
        /// Used to get all entities.
        /// </summary>
        /// <param name="options">Query options</param>
        /// <param name="totalEntities">Gets count of all entities based on the given <paramref name="options"/></param>
        /// <returns>List of all entities</returns>
        List<TEntity> GetAllList(Options.GetAllOptions<TEntity, TPrimaryKey> options, out long totalEntities);

        /// <summary>
        /// Used to get all entities.
        /// </summary>
        /// <param name="options">Query options</param>
        /// <param name="totalEntities">Gets count of all entities based on the given <paramref name="options"/></param>
        /// <returns>List of all entities</returns>
        Task<List<TEntity>> GetAllListAsync(Options.GetAllOptions<TEntity, TPrimaryKey> options, out long totalEntities);

        /// <summary>
        /// Used to get all entities based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <returns>List of all entities</returns>
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate, Options.GetAllOptions<TEntity, TPrimaryKey> options);

        /// <summary>
        /// Used to get all entities based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <returns>List of all entities</returns>
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate, Options.GetAllOptions<TEntity, TPrimaryKey> options);

        /// <summary>
        /// Used to get all entities based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <param name="totalEntities">Gets count of all entities based on the given <paramref name="options"/></param>
        /// <returns>List of all entities</returns>
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate, Options.GetAllOptions<TEntity, TPrimaryKey> options, out long totalEntities);

        /// <summary>
        /// Used to get all entities based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <param name="totalEntities">Gets count of all entities based on the given <paramref name="options"/></param>
        /// <returns>List of all entities</returns>
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate, Options.GetAllOptions<TEntity, TPrimaryKey> options, out long totalEntities);

        /// <summary>
        /// Used to get all entities projected to a custom type.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="options">Query options</param>
        /// <returns>List of all entities projected to <typeparamref name="TProjection"/> type</returns>
        List<TProjection> GetAllList<TProjection>(Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class;

        /// <summary>
        /// Used to get all entities projected to a custom type.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="options">Query options</param>
        /// <returns>List of all entities projected to <typeparamref name="TProjection"/> type</returns>
        Task<List<TProjection>> GetAllListAsync<TProjection>(Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class;

        /// <summary>
        /// Used to get all entities projected to a custom type.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="options">Query options</param>
        /// <param name="totalEntities">Gets count of all entities based on the given <paramref name="options"/></param>
        /// <returns>List of all entities projected to <typeparamref name="TProjection"/> type</returns>
        List<TProjection> GetAllList<TProjection>(Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options, out long totalEntities) where TProjection : class;

        /// <summary>
        /// Used to get all entities projected to a custom type.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="options">Query options</param>
        /// <param name="totalEntities">Gets count of all entities based on the given <paramref name="options"/></param>
        /// <returns>List of all entities projected to <typeparamref name="TProjection"/> type</returns>
        Task<List<TProjection>> GetAllListAsync<TProjection>(Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options, out long totalEntities) where TProjection : class;

        /// <summary>
        /// Used to get all entities projected to a custom type based on given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <returns>List of all entities projected to <typeparamref name="TProjection"/> type</returns>
        List<TProjection> GetAllList<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class;

        /// <summary>
        /// Used to get all entities projected to a custom type based on given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <returns>List of all entities projected to <typeparamref name="TProjection"/> type</returns>
        Task<List<TProjection>> GetAllListAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class;

        /// <summary>
        /// Used to get all entities projected to a custom type based on given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <param name="totalEntities">Gets count of all entities based on the given <paramref name="options"/></param>
        /// <returns>List of all entities projected to <typeparamref name="TProjection"/> type</returns>
        List<TProjection> GetAllList<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options, out long totalEntities) where TProjection : class;

        /// <summary>
        /// Used to get all entities projected to a custom type based on given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <param name="totalEntities">Gets count of all entities based on the given <paramref name="options"/></param>
        /// <returns>List of all entities projected to <typeparamref name="TProjection"/> type</returns>
        Task<List<TProjection>> GetAllListAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetAllProjectedOptions<TEntity, TPrimaryKey, TProjection> options, out long totalEntities) where TProjection : class;

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity</returns>
        TEntity Get(TPrimaryKey id, Options.GetOptions<TEntity, TPrimaryKey> options);

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity</returns>
        Task<TEntity> GetAsync(TPrimaryKey id, Options.GetOptions<TEntity, TPrimaryKey> options);

        /// <summary>
        /// Gets an entity with given primary key projected to a custom type.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="id">Primary key of the entity to get</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity projected to <typeparamref name="TProjection"/> type</returns>
        TProjection Get<TProjection>(TPrimaryKey id, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class;

        /// <summary>
        /// Gets an entity with given primary key projected to a custom type.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="id">Primary key of the entity to get</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity projected to <typeparamref name="TProjection"/> type</returns>
        Task<TProjection> GetAsync<TProjection>(TPrimaryKey id, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class;

        /// <summary>
        /// Gets exactly one entity with given predicate.
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity</returns>
        TEntity Single(Expression<Func<TEntity, bool>> predicate, Options.GetOptions<TEntity, TPrimaryKey> options);

        /// <summary>
        /// Gets exactly one entity with given predicate.
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity</returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, Options.GetOptions<TEntity, TPrimaryKey> options);

        /// <summary>
        /// Gets exactly one entity with given predicate projected to a custom type.
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity projected to <typeparamref name="TProjection"/> type</returns>
        TProjection Single<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class;

        /// <summary>
        /// Gets exactly one entity with given predicate projected to a custom type.
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity projected to <typeparamref name="TProjection"/> type</returns>
        Task<TProjection> SingleAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class;

        /// <summary>
        /// Gets an entity with given primary key or null if not found.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity or null</returns>
        TEntity FirstOrDefault(TPrimaryKey id, Options.GetOptions<TEntity, TPrimaryKey> options);

        /// <summary>
        ///Gets an entity with given primary key or null if not found.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity or null</returns>
        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id, Options.GetOptions<TEntity, TPrimaryKey> options);

        /// <summary>
        /// Gets an entity with given primary key projected to a custom type or null if not found.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="id">Primary key of the entity to get</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity projected to <typeparamref name="TProjection"/> type or null</returns>
        TProjection FirstOrDefault<TProjection>(TPrimaryKey id, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class;

        /// <summary>
        /// Gets an entity with given primary key projected to a custom type or null if not found.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="id">Primary key of the entity to get</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity projected to <typeparamref name="TProjection"/> type or null</returns>
        Task<TProjection> FirstOrDefaultAsync<TProjection>(TPrimaryKey id, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class;

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity or null</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, Options.GetOptions<TEntity, TPrimaryKey> options);

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity or null</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, Options.GetOptions<TEntity, TPrimaryKey> options);

        /// <summary>
        /// Gets an entity with given given predicate projected to a custom type or null if not found.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity projected to <typeparamref name="TProjection"/> type or null</returns>
        TProjection FirstOrDefault<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class;

        /// <summary>
        /// Gets an entity with given given predicate projected to a custom type or null if not found.
        /// </summary>
        /// <typeparam name="TProjection">Projection Type</typeparam>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="options">Query options</param>
        /// <returns>Entity projected to <typeparamref name="TProjection"/> type or null</returns>
        Task<TProjection> FirstOrDefaultAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Options.GetProjectedOptions<TEntity, TPrimaryKey, TProjection> options) where TProjection : class;

        #endregion Select/Get/Query

        #region Insert

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts a new entity and gets it's Id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        TPrimaryKey InsertAndGetId(TEntity entity);

        /// <summary>
        /// Inserts a new entity and gets it's Id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// </summary>
        /// <param name="entity">Entity</param>
        TEntity InsertOrUpdate(TEntity entity);

        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<TEntity> InsertOrUpdateAsync(TEntity entity);

        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// Also returns Id of the entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        TPrimaryKey InsertOrUpdateAndGetId(TEntity entity);

        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// Also returns Id of the entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity);

        #endregion Insert

        #region Update

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Updates an existing entity. 
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<TEntity> UpdateAsync(TEntity entity);

        #endregion Update

        #region Delete

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity</param>
        void Delete(TPrimaryKey id);

        /// <summary>
        /// Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity</param>
        Task DeleteAsync(TPrimaryKey id);

        /// <summary>
        /// Deletes many entities by function.
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Deletes many entities by function.
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion Delete

        #region Aggregates

        /// <summary>
        /// Gets count of all entities in this repository.
        /// </summary>
        /// <returns>Count of entities</returns>
        long Count();

        /// <summary>
        /// Gets count of all entities in this repository.
        /// </summary>
        /// <returns>Count of entities</returns>
        Task<long> CountAsync();

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        long Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Checks if an entity with given primary key exists.
        /// </summary>
        /// <param name="id">Primary key of the entity to check</param>
        /// <returns>True if exists</returns>
        bool Exists(TPrimaryKey id);

        /// <summary>
        /// Checks if an entity with given primary key exists.
        /// </summary>
        /// <param name="id">Primary key of the entity to check</param>
        /// <returns>True if exists</returns>
        Task<bool> ExistsAsync(TPrimaryKey id);
        
        /// <summary>
        /// Checks if an entity with given <paramref name="predicate"/> exists.
        /// </summary>
        /// <param name="predicate">A method to filter check</param>
        /// <returns>True if exists</returns>
        bool Exists(Expression<Func<TEntity, bool>> predicate);
        
        /// <summary>
        /// Checks if an entity with given <paramref name="predicate"/> exists.
        /// </summary>
        /// <param name="predicate">A method to filter check</param>
        /// <returns>True if exists</returns>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion Aggregates
    }

    public interface IRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : class, IEntity
    {
    }
}
