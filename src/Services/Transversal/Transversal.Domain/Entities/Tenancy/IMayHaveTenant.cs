using System.Collections.Generic;

namespace Transversal.Domain.Entities.Tenancy
{
    /// <summary>
    /// Interface implemented by entities which may optionally have TenantId.
    /// </summary>
    /// <typeparam name="TEntity">Type of this entity</typeparam>
    /// <typeparam name="TPrimaryKey">Type of the primary key of this entity</typeparam>
    public interface IMayHaveTenant<TEntity, TPrimaryKey>
        where TEntity : IEntity<TPrimaryKey>, IMayHaveTenant<TEntity, TPrimaryKey>
    {
        /// <summary>
        /// TenantId of this entity
        /// </summary>
        int? TenantId { get; set; }

        /// <summary>
        /// On an tenant overridden entity, this is the reference for the tenantless entity
        /// </summary>
        TEntity TenantlessEntity { get; set; }


        /// <summary>
        /// On an tenantless entity, this has the collection of the tenant overridden entities
        /// </summary>
        ICollection<TEntity> TenantEntities { get; set; }
    }
}
