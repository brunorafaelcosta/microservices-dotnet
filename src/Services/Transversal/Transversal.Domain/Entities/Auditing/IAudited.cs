namespace Transversal.Domain.Entities.Auditing
{
    /// <summary>
    /// Interface implemented by entities which must be audited.
    /// </summary>
    /// <remarks>
    /// Related properties are automatically set when saving/updating <see cref="IEntity{TPrimaryKey}"/> objects.
    /// </remarks>
    public interface IAudited :
        ICreationAudited,
        IModificationAudited,
        IDeletionAudited
    {
    }
}
