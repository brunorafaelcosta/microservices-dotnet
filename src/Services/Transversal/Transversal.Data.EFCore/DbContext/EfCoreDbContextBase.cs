using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Diagnostics;
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
                typeof(EfCoreDbContextBase)
                    .GetMethod(nameof(ApplyBaseFilters), BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
        }

        #region Entity base filters

        protected virtual bool IsSoftDeleteBaseFilterEnabled => CurrentUnitOfWorkProvider?.Current?.IsSoftDeleteBaseFilterEnabled ?? true;
        protected virtual bool IsMayHaveTenantBaseFilterEnabled => CurrentUnitOfWorkProvider?.Current.IsMayHaveTenantBaseFilterEnabled ?? true;
        protected virtual bool IsMustHaveTenantBaseFilterEnabled => CurrentUnitOfWorkProvider?.Current?.IsMustHaveTenantBaseFilterEnabled ?? true;

        protected virtual void ApplyBaseFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
            where TEntity : class
        {
            var shouldApplyBaseFilters =
                typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity))
                || typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity))
                || typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity));

            if (entityType.BaseType == null && shouldApplyBaseFilters)
            {
                var baseFiltersExpression = CreateBaseFiltersExpression<TEntity>();
                if (baseFiltersExpression != null)
                {
                    modelBuilder.Entity<TEntity>().HasQueryFilter(baseFiltersExpression);
                }
            }
        }

        protected virtual Expression<Func<TEntity, bool>> CreateBaseFiltersExpression<TEntity>()
            where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                /* This condition should normally be defined as below:
                 * !IsSoftDeleteFilterEnabled || !((ISoftDelete) e).IsDeleted
                 * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
                 * So, we made a workaround to make it working. It works same as above.
                 */

                Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete)e).IsDeleted || ((ISoftDelete)e).IsDeleted != IsSoftDeleteBaseFilterEnabled;
                expression = expression == null ? softDeleteFilter : expression.CombineWithExpression(softDeleteFilter);
            }

            if (typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity)))
            {
                /* This condition should normally be defined as below:
                 * !IsMayHaveTenantFilterEnabled || ((IMayHaveTenant)e).TenantId == CurrentTenantId
                 * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
                 * So, we made a workaround to make it working. It works same as above.
                 */
                Expression<Func<TEntity, bool>> mayHaveTenantFilter = e => ((IMayHaveTenant)e).TenantId == CurrentTenantId || (((IMayHaveTenant)e).TenantId == CurrentTenantId) == IsMayHaveTenantBaseFilterEnabled;
                expression = expression == null ? mayHaveTenantFilter : expression.CombineWithExpression(mayHaveTenantFilter);
            }

            if (typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity)))
            {
                /* This condition should normally be defined as below:
                 * !IsMustHaveTenantFilterEnabled || ((IMustHaveTenant)e).TenantId == CurrentTenantId
                 * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
                 * So, we made a workaround to make it working. It works same as above.
                 */
                Expression<Func<TEntity, bool>> mustHaveTenantFilter = e => ((IMustHaveTenant)e).TenantId == CurrentTenantId || (((IMustHaveTenant)e).TenantId == CurrentTenantId) == IsMustHaveTenantBaseFilterEnabled;
                expression = expression == null ? mustHaveTenantFilter : expression.CombineWithExpression(mustHaveTenantFilter);
            }

            return expression;
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
