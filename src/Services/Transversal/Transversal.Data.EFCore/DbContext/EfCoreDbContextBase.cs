using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Transversal.Common.Extensions;
using Transversal.Common.Extensions.Linq;
using Transversal.Domain.Entities;
using Transversal.Domain.Entities.Tenancy;
using Transversal.Domain.Uow.Provider;

namespace Transversal.Data.EFCore.DbContext
{
    /// <summary>
    /// Base class for all DbContexts in the application.
    /// </summary>
    public abstract class EfCoreDbContextBase : Microsoft.EntityFrameworkCore.DbContext
    {
        protected virtual IDbContextInterceptor Interceptor { get; set; }
        protected virtual ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

        #region Current user info

        protected virtual long? CurrentUserId => CurrentUnitOfWorkProvider?.Current?.CurrentUserId;
        protected virtual int? CurrentTenantId => CurrentUnitOfWorkProvider?.Current?.CurrentTenantId;
        protected virtual string CurrentLanguageCode => CurrentUnitOfWorkProvider?.Current?.CurrentLanguageCode;

        protected virtual string GetCurrentLanguageCode() => CurrentLanguageCode;

        #endregion Current user info

        protected EfCoreDbContextBase(
            DbContextOptions options,
            IDbContextInterceptor interceptor,
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
            : base(options)
        {
            Interceptor = interceptor;
            CurrentUnitOfWorkProvider = currentUnitOfWorkProvider;

            InitializeDbContext();
        }

        protected virtual void InitializeDbContext()
        {
            Interceptor.GetCurrentLanguageCode = GetCurrentLanguageCode;
            this.GetService<DiagnosticSource>()
                .As<DiagnosticListener>()
                .SubscribeWithAdapter(Interceptor);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Owned<Domain.ValueObjects.Localization.LocalizedValueObject>();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                Type TEntity = entityType.ClrType;
                Type TPrimaryKey = TEntity.GetProperty("Id")?.PropertyType;

                if (TPrimaryKey is null)
                    continue;

                if (EntityExtensions.EntityIsMayHaveTenant(TEntity, TPrimaryKey))
                {
                    modelBuilder.Entity(TEntity)
                        .HasOne(TEntity, "TenantlessEntity")
                        .WithMany("TenantEntities")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasForeignKey("TenantlessEntityId")
                        .IsRequired(false);
                }

                typeof(EfCoreDbContextBase)
                    .GetMethod(nameof(ApplyBaseFilters), BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(TEntity, TPrimaryKey)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
        }

        #region Entity base filters

        protected virtual bool IsSoftDeleteBaseFilterEnabled => CurrentUnitOfWorkProvider?.Current?.IsSoftDeleteBaseFilterEnabled ?? true;
        protected virtual bool IsMayHaveTenantBaseFilterEnabled => CurrentUnitOfWorkProvider?.Current.IsMayHaveTenantBaseFilterEnabled ?? true;
        protected virtual bool IsMustHaveTenantBaseFilterEnabled => CurrentUnitOfWorkProvider?.Current?.IsMustHaveTenantBaseFilterEnabled ?? true;

        protected virtual void ApplyBaseFilters<TEntity, TPrimaryKey>(ModelBuilder modelBuilder, IMutableEntityType entityType)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            bool isSoftDelete = typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity));
            bool isMayHaveTenantType = EntityExtensions.EntityIsMayHaveTenant(typeof(TEntity), typeof(TPrimaryKey));
            bool isMustHaveTenant = typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity));

            bool shouldApplyBaseFilters() => isSoftDelete || isMayHaveTenantType || isMustHaveTenant;
            if (entityType.BaseType == null && shouldApplyBaseFilters())
            {
                var baseFiltersExpression = CreateBaseFiltersExpression<TEntity, TPrimaryKey>(
                    isSoftDelete,
                    isMayHaveTenantType,
                    isMustHaveTenant
                    );
                if (baseFiltersExpression != null)
                {
                    modelBuilder.Entity<TEntity>().HasQueryFilter(baseFiltersExpression);
                }
            }
        }

        protected virtual Expression<Func<TEntity, bool>> CreateBaseFiltersExpression<TEntity, TPrimaryKey>(
            bool isSoftDelete,
            bool isMayHaveTenantType,
            bool isMustHaveTenant)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            Type entityType = typeof(TEntity);
            Type primaryKeyType = typeof(TPrimaryKey);

            Expression<Func<TEntity, bool>> expression = null;

            if (isSoftDelete)
            {
                /* 
                 * https://github.com/aspnet/EntityFrameworkCore/issues/9502
                 */
                Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete)e).IsDeleted || ((ISoftDelete)e).IsDeleted != IsSoftDeleteBaseFilterEnabled;
                expression = expression == null ? softDeleteFilter : expression.CombineWithExpression(softDeleteFilter);
            }

            if (isMayHaveTenantType)
            {
                Expression<Func<TEntity, bool>> mayHaveTenantFilter = typeof(EfCoreDbContextBase)
                    .GetMethod(nameof(CreateMayHaveTenantBaseFiltersExpression), BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(entityType, primaryKeyType)
                    .Invoke(this, null)
                    .As<Expression<Func<TEntity, bool>>>();

                expression = expression == null ? mayHaveTenantFilter : expression.CombineWithExpression(mayHaveTenantFilter);
            }

            if (isMustHaveTenant)
            {
                /* 
                 * https://github.com/aspnet/EntityFrameworkCore/issues/9502
                 */
                Expression<Func<TEntity, bool>> mustHaveTenantFilter = e =>
                    CurrentTenantId == null
                    || (
                        (((IMustHaveTenant)e).TenantId == CurrentTenantId
                            || (((IMustHaveTenant)e).TenantId == CurrentTenantId) == IsMustHaveTenantBaseFilterEnabled)
                    );
                
                expression = expression == null ? mustHaveTenantFilter : expression.CombineWithExpression(mustHaveTenantFilter);
            }

            return expression;
        }

        protected virtual Expression<Func<TEntity, bool>> CreateMayHaveTenantBaseFiltersExpression<TEntity, TPrimaryKey>()
            where TEntity : class, IEntity<TPrimaryKey>, IMayHaveTenant<TEntity, TPrimaryKey>
        {
            /* 
             * https://github.com/aspnet/EntityFrameworkCore/issues/9502
             */
            Expression<Func<TEntity, bool>> mayHaveTenantFilter = e =>
                (CurrentTenantId == null
                || (CurrentTenantId != null &
                    (e.TenantId == null
                        && (!e.TenantEntities.AsQueryable().IgnoreQueryFilters().Any(te => te.TenantId == CurrentTenantId)))
                    || (
                        (e.TenantId != null && e.TenantId == CurrentTenantId)
                        || ((e.TenantId == CurrentTenantId) == IsMayHaveTenantBaseFilterEnabled)
                        )
                    )
                );

            return mayHaveTenantFilter;
        }

        #endregion Entity base filters

        public override int SaveChanges()
        {
            try
            {
                var result = base.SaveChanges();
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
        }
    }
}
