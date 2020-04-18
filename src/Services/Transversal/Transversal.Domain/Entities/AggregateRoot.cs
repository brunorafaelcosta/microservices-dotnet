namespace Transversal.Domain.Entities
{
    public abstract class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>,
        IAggregateRoot<TPrimaryKey>
    {
    }

    public abstract class AggregateRoot : AggregateRoot<int>,
        IAggregateRoot
    {
    }
}
