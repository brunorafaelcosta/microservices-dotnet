namespace Transversal.Data.EFCore
{
    /// <summary>
    /// Interface used by <see cref="Repositories.EFCoreRepositoryBase{TDbContext, TEntity, TPrimaryKey}"/>
    /// to retrieve the current DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">Type of the DbContext</typeparam>
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : DbContextBase
    {
        TDbContext GetDbContext();
    }
}
