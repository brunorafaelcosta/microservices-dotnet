using System;

namespace Transversal.Domain.Entities.Auditing
{
    /// <summary>
    /// Interface implemented by entities which is wanted to store deletion information (who and when deleted).
    /// </summary>
    /// <remarks>
    /// Related properties are automatically set when saving <see cref="IEntity{TPrimaryKey}"/> objects.
    /// </remarks>
    public interface IDeletionAudited
    {
        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        long? DeleterUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        DateTime? DeletionTime { get; set; }
    }
}
