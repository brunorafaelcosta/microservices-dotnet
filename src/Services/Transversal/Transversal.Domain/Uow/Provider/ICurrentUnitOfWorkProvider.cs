namespace Transversal.Domain.Uow.Provider
{
    /// <summary>
    /// Used to get/set the current <see cref="IUnitOfWork"/>. 
    /// </summary>
    public interface ICurrentUnitOfWorkProvider
    {
        /// <summary>
        /// Current <see cref="IUnitOfWork"/>.
        /// <para>Setting to null returns back to outer Unit of Work when possible.</para>
        /// </summary>
        IUnitOfWork Current { get; set; }
    }
}
