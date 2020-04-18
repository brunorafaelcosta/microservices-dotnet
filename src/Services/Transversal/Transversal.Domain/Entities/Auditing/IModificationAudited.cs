using System;

namespace Transversal.Domain.Entities.Auditing
{
    /// <summary>
    /// Interface implemented by entities which is wanted to store modification information (who and when modified lastly).
    /// </summary>
    /// <remarks>
    /// Related properties are automatically set when updating <see cref="IEntity{TPrimaryKey}"/> objects.
    /// </remarks>
    public interface IModificationAudited
    {
        /// <summary>
        /// Last modifier user for this entity.
        /// </summary>
        long? LastModifierUserId { get; set; }

        /// <summary>
        /// The last modified time for this entity.
        /// </summary>
        DateTime? LastModificationTime { get; set; }
    }
}
