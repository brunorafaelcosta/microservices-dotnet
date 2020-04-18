namespace Transversal.Domain.Entities
{
    /// <summary>
    /// Interface implemented by entities which can be soft deleted.
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Used to mark an entity as soft deleted. 
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
