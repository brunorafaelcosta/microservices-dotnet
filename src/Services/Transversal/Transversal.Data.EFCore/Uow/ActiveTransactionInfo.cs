using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage;

namespace Transversal.Data.EFCore.Uow
{
    /// <summary>
    /// Holds info of the active transaction in the current Unit Of Work.
    /// </summary>
    public class ActiveTransactionInfo
    {
        public IDbContextTransaction DbContextTransaction { get; }

        public DbContextBase StarterDbContext { get; }

        public List<DbContextBase> AttendedDbContexts { get; }

        public ActiveTransactionInfo(IDbContextTransaction dbContextTransaction, DbContextBase starterDbContext)
        {
            DbContextTransaction = dbContextTransaction;
            StarterDbContext = starterDbContext;

            AttendedDbContexts = new List<DbContextBase>();
        }
    }
}
