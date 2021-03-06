using System.Collections.Generic;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Transversal.Common.InversionOfControl;
using Transversal.Common.Extensions.Collections;
using Transversal.Data.EFCore.DbContext;
using Transversal.Data.EFCore.Extensions.Transactions;
using Transversal.Domain.Uow.Options;

namespace Transversal.Data.EFCore.Uow
{
    public class DbContextEfCoreTransactionStrategy : IEfCoreTransactionStrategy
    {
        protected UnitOfWorkOptions Options { get; private set; }

        protected IDictionary<string, ActiveTransactionInfo> ActiveTransactions { get; }

        public DbContextEfCoreTransactionStrategy()
        {
            ActiveTransactions = new Dictionary<string, ActiveTransactionInfo>();
        }

        public void InitOptions(UnitOfWorkOptions options)
        {
            Options = options;
        }

        public TDbContext CreateDbContext<TDbContext>(string dbContextKey, IDbContextResolver dbContextResolver)
            where TDbContext : EfCoreDbContextBase
        {
            TDbContext dbContext;

            var activeTransaction = ActiveTransactions.GetOrDefault(dbContextKey);
            if (activeTransaction == null)
            {
                dbContext = dbContextResolver.Resolve<TDbContext>();

                var dbtransaction = dbContext.Database.BeginTransaction((Options.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());
                activeTransaction = new ActiveTransactionInfo(dbtransaction, dbContext);
                ActiveTransactions[dbContextKey] = activeTransaction;
            }
            else
            {
                dbContext = dbContextResolver.Resolve<TDbContext>(
                    activeTransaction.DbContextTransaction.GetDbTransaction().Connection
                );

                if (dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager)
                {
                    dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.GetDbTransaction());
                }
                else
                {
                    dbContext.Database.BeginTransaction();
                }

                activeTransaction.AttendedDbContexts.Add(dbContext);
            }

            return dbContext;
        }

        public void Commit()
        {
            foreach (var activeTransaction in ActiveTransactions.Values)
            {
                activeTransaction.DbContextTransaction.Commit();

                foreach (var dbContext in activeTransaction.AttendedDbContexts)
                {
                    if (dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager)
                    {
                        continue; //Relational databases use the shared transaction
                    }

                    dbContext.Database.CommitTransaction();
                }
            }
        }

        public void Dispose(IIoCResolver iocResolver)
        {
            foreach (var activeTransaction in ActiveTransactions.Values)
            {
                activeTransaction.DbContextTransaction.Dispose();

                foreach (var attendedDbContext in activeTransaction.AttendedDbContexts)
                {
                    iocResolver.Release(attendedDbContext);
                }

                iocResolver.Release(activeTransaction.StarterDbContext);
            }

            ActiveTransactions.Clear();
        }
    }
}
