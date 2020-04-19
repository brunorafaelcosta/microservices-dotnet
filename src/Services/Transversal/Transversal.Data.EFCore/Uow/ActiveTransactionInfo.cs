using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage;
using Transversal.Data.EFCore.DbContext;

namespace Transversal.Data.EFCore.Uow
{
    /// <summary>
    /// Holds info of the active transaction in the current Unit Of Work.
    /// </summary>
    public class ActiveTransactionInfo
    {
        public IDbContextTransaction DbContextTransaction { get; }

        public EfCoreDbContextBase StarterDbContext { get; }

        public List<EfCoreDbContextBase> AttendedDbContexts { get; }

        public ActiveTransactionInfo(IDbContextTransaction dbContextTransaction, EfCoreDbContextBase starterDbContext)
        {
            DbContextTransaction = dbContextTransaction;
            StarterDbContext = starterDbContext;

            AttendedDbContexts = new List<EfCoreDbContextBase>();
        }
    }
}
