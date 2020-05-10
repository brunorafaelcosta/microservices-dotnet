using System;

namespace Transversal.Domain.Uow
{
    /// <summary>
    /// Interface used to work with the active unit of work.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Manager.IUnitOfWorkManager.Begin()"/> to start a new Unit of Work.
    /// </remarks>
    public interface IActiveUnitOfWork
    {
        #region Events

        /// <summary>
        /// This event is raised when this Unit of Work is successfully completed.
        /// </summary>
        event EventHandler Completed;

        /// <summary>
        /// This event is raised when this Unit of Work is failed.
        /// </summary>
        event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <summary>
        /// This event is raised when this Unit of Work is disposed.
        /// </summary>
        event EventHandler Disposed;

        #endregion Events

        #region Current user info

        /// <summary>
        /// User id of this Unit of Work.
        /// </summary>
        /// <remarks>
        /// Normally, it will be the authenticated user id unless 
        /// it's overridden by <see cref="OverrideCurrentUserId(int?)"/> method.
        /// </remarks>
        long? CurrentUserId { get; }

        /// <summary>
        /// Tenant id of this Unit of Work.
        /// </summary>
        /// <remarks>
        /// Normally, it will be the authenticated user tenant id unless 
        /// it's overridden by <see cref="OverrideCurrentTenantId(int?)"/> method.
        /// </remarks>
        int? CurrentTenantId { get; }

        /// <summary>
        /// Language code of this Unit of Work.
        /// </summary>
        /// <remarks>
        /// Normally, it will be the authenticated user language code unless 
        /// it's overridden by <see cref="OverrideCurrentLanguageCode(string)"/> method.
        /// </remarks>
        string CurrentLanguageCode { get; }

        /// <summary>
        /// Temporarily overrides the current user id.
        /// </summary>
        /// <param name="overriddenUserId">Overridden user id</param>
        /// <returns>Disposable action to restore the previous user id</returns>
        IDisposable OverrideCurrentUserId(int? overriddenUserId);

        /// <summary>
        /// Temporarily overrides the current tenant id.
        /// </summary>
        /// <param name="overriddenTenantId">Overridden tenant id</param>
        /// <param name="disableMustHaveTenantBaseFilter">If 'must have tenant' base filter is to be temporarily disabled</param>
        /// <returns>Disposable action to restore the previous tenant id</returns>
        IDisposable OverrideCurrentTenantId(int? overriddenTenantId, bool disableMustHaveTenantBaseFilter = true);

        /// <summary>
        /// Temporarily overrides the current language code.
        /// </summary>
        /// <param name="overriddenLanguageCode">Overridden language code</param>
        /// <returns>Disposable action to restore the previous language code</returns>
        IDisposable OverrideCurrentLanguageCode(string overriddenLanguageCode);

        #endregion Current user info

        #region Entity base filters

        /// <summary>
        /// Gets if the 'soft delete' base filter is enabled.
        /// </summary>
        bool IsSoftDeleteBaseFilterEnabled { get; }

        /// <summary>
        /// Gets if the 'may have tenant' base filter is enabled.
        /// </summary>
        bool IsMayHaveTenantBaseFilterEnabled { get; }

        /// <summary>
        /// Gets if the 'must have tenant' base filter is enabled.
        /// </summary>
        bool IsMustHaveTenantBaseFilterEnabled { get; }

        /// <summary>
        /// Temporarily disables the 'soft delete' base filter.
        /// </summary>
        /// <returns>Disposable action to enable the 'soft delete' base filter</returns>
        IDisposable DisableSoftDeleteBaseFilter();

        /// <summary>
        /// Temporarily disables the 'may have tenant' base filter.
        /// </summary>
        /// <returns>Disposable action to enable the 'may have tenant' base filter</returns>
        IDisposable DisableMayHaveTenantBaseFilter();

        /// <summary>
        /// Temporarily disables the 'must have tenant' base filter.
        /// </summary>
        /// <returns>Disposable action to enable the 'must have tenant' base filter</returns>
        IDisposable DisableMustHaveTenantBaseFilter();

        #endregion Entity base filters
    }
}
