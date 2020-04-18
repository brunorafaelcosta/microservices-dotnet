namespace Transversal.Domain.Entities
{
    /// <summary>
    /// Interface implemented by the root entity in a cluster of entities that can be treated as a single unit.
    /// </summary>
    /// <remarks>
    /// Any references from outside the aggregate should only go to the aggregate root.
    /// The root can thus ensure the integrity of the aggregate as a whole.
    /// </remarks>
    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>
    {
    }
    
    public interface IAggregateRoot : IAggregateRoot<int>, IEntity
    {
    }
}
