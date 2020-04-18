using System;

namespace Transversal.Domain.Entities.Auditing
{
    /// <summary>
    /// Interface implemented by entities which is wanted to store creation information (who and when created).
    /// </summary>
    /// <remarks>
    /// Related properties are automatically set when saving <see cref="IEntity{TPrimaryKey}"/> objects.
    /// </remarks>
    public interface ICreationAudited
    {
        /// <summary>
        /// Id of the creator user of this entity.
        /// </summary>
        long CreatorUserId { get; set; }

        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        DateTime CreationTime { get; set; }
    }
}
