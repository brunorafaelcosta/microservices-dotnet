namespace Transversal.Domain.Entities.Tenancy
{
    /// <summary>
    /// Interface implemented by entities which must have TenantId.
    /// </summary>
    public interface IMustHaveTenant
    {
        /// <summary>
        /// TenantId of this entity.
        /// </summary>
        int TenantId { get; set; }
    }
}
