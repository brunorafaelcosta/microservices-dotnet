namespace Transversal.Domain.Entities
{
    public abstract class AuditedAggregateRoot<TPrimaryKey> : AuditedEntity<TPrimaryKey>,
        IAggregateRoot<TPrimaryKey>
    {
    }

    public abstract class AuditedAggregateRoot : AuditedAggregateRoot<int>,
        IAggregateRoot
    {
    }
}
