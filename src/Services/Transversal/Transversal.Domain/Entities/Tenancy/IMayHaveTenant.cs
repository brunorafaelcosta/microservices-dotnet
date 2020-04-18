namespace Transversal.Domain.Entities.Tenancy
{
    /// <summary>
    /// Interface implemented by entities which may optionally have TenantId.
    /// </summary>
    public interface IMayHaveTenant
    {
        /// <summary>
        /// TenantId of this entity.
        /// </summary>
        int? TenantId { get; set; }
    }
}
