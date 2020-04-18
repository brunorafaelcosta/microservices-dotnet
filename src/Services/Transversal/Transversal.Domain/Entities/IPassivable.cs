namespace Transversal.Domain.Entities
{
    /// <summary>
    /// Interface implemented by entities which can be active/passive.
    /// </summary>
    public interface IPassivable
    {
        /// <summary>
        /// <para>True: This entity is active.</para>
        /// <para>False: This entity isn't active.</para>
        /// </summary>
        bool IsActive { get; set; }
    }
}
