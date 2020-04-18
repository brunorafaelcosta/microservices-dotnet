using System;
using Transversal.Domain.Uow;

namespace Transversal.Data.EFCore.Uow
{
    /// <summary>
    /// Extension methods for <see cref="IUnitOfWork"/> objects.
    /// </summary>
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// Gets a DbContext from the current Unit of Work.
        /// This method can be called when current Unit of Work is based on <see cref="EfCoreUnitOfWork"/>.
        /// </summary>
        /// <typeparam name="TDbContext">Type of the DbContext</typeparam>
        /// <param name="unitOfWork">Current (active) Unit of Work</param>
        public static TDbContext GetDbContext<TDbContext>(this IUnitOfWork unitOfWork)
            where TDbContext : DbContextBase
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            if (!(unitOfWork is EfCoreUnitOfWork))
            {
                throw new ArgumentException($"{nameof(unitOfWork)} is not type of {typeof(EfCoreUnitOfWork).FullName}", nameof(unitOfWork));
            }

            return (unitOfWork as EfCoreUnitOfWork).GetOrCreateDbContext<TDbContext>();
        }
    }
}
