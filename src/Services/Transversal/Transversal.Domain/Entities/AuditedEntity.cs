using System;

namespace Transversal.Domain.Entities
{
    public abstract class AuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>,
        Auditing.IAudited
    {
        public long CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }

    public abstract class AuditedEntity : AuditedEntity<int>,
        Auditing.IAudited
    {
    }
}
