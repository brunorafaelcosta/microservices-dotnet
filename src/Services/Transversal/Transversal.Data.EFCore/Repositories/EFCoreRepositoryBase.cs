using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Transversal.Data.EFCore.DbContext;
using Transversal.Domain.Entities;
using Transversal.Domain.Repositories;

namespace Transversal.Data.EFCore.Repositories
{
    public abstract class EFCoreRepositoryBase<TDbContext, TEntity, TPrimaryKey> : 
        RepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : EfCoreDbContextBase
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        protected virtual TDbContext Context => _dbContextProvider.GetDbContext();
        protected virtual DbSet<TEntity> Entities => Context.Set<TEntity>();

        protected override IQueryable<TEntity> AsQueryable()
        {
            return Entities.AsQueryable();
        }

        public EFCoreRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider ?? throw new ArgumentNullException(nameof(dbContextProvider));
        }

        #region Select/Get/Query

        #endregion Select/Get/Query

        #region Insert

        public override TEntity Insert(TEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));
            
            entity.Id = GetNextValueId();

            return Entities.Add(entity).Entity;
        }

        #endregion Insert

        #region Update

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        #endregion Update

        #region Delete

        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Entities.Remove(entity);
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = FirstOrDefault(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            // Couldn't find the entity, do nothing.
        }

        #endregion Delete
        
        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = Context.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            Entities.Attach(entity);
        }
        
        protected virtual TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = Context.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, (ent.Entity as TEntity).Id)
                );

            return entry?.Entity as TEntity;
        }
    
        protected virtual TPrimaryKey GetNextValueId()
        {
            return default(TPrimaryKey);
        }
    
    }
}
