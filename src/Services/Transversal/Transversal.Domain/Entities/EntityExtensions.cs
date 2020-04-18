using Transversal.Common.Extensions;
using Transversal.Domain.Entities.Auditing;

namespace Transversal.Domain.Entities
{
    /// <summary>
    /// Extension methods for <see cref="IEntity{TPrimaryKey}"/> objects.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Checks if this entity is null or soft deleted.
        /// </summary>
        public static bool IsNullOrDeleted(this ISoftDelete entity)
        {
            return entity == null || entity.IsDeleted;
        }

        /// <summary>
        /// Undeletes this entity by reseting <see cref="ISoftDelete"/> and <see cref="IDeletionAudited"/> properties.
        /// </summary>
        public static void UnDelete(this ISoftDelete entity)
        {
            entity.IsDeleted = false;

            if (entity is IDeletionAudited)
            {
                var deletionAuditedEntity = entity.As<IDeletionAudited>();
                deletionAuditedEntity.DeletionTime = null;
                deletionAuditedEntity.DeleterUserId = null;
            }
        }
    }
}
