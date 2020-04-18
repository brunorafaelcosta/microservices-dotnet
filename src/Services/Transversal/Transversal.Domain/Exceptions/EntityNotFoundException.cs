using System;

namespace Transversal.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown if an entity expected to be found, doesn't exist in the repository.
    /// </summary>
    public class EntityNotFoundException : DomainException
    {
        const string EntityNotFoundExceptionMessage = "There is no such entity. Entity type: {0}, id: {1}";

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(Type entityType, object id)
            : this(entityType, id, null)
        {
        }

        public EntityNotFoundException(Type entityType, object id, Exception innerException)
            : base(string.Format(EntityNotFoundExceptionMessage, entityType.FullName, id), innerException)
        {
            EntityType = entityType;
            Id = id;
        }

        /// <summary>
        /// Type of the entity.
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// Id of the Entity.
        /// </summary>
        public object Id { get; set; }
    }
}
