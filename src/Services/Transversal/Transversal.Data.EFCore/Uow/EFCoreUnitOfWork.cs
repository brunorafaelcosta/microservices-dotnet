using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Transversal.Common.Dependency;
using Transversal.Common.Session;
using Transversal.Data.EFCore.DbContext;
using Transversal.Domain.Uow;

namespace Transversal.Data.EFCore.Uow
{
    /// <summary>
    /// Base class for all <see cref="IUnitOfWork"/> in the application using Entity Framework.
    /// </summary>
    public abstract class EfCoreUnitOfWork : UnitOfWorkBase
    {
        protected IDictionary<string, EfCoreDbContextBase> ActiveDbContexts { get; }
        protected IIocResolver IocResolver { get; }
        protected IEfCoreTransactionStrategy TransactionStrategy { get; }
        protected IDbContextResolver DbContextResolver { get; }

        public EfCoreUnitOfWork(
            IIocResolver iocResolver,
            IEfCoreTransactionStrategy transactionStrategy,
            IDbContextResolver dbContextResolver,
            IConnectionStringResolver connectionStringResolver,
            ISessionInfo sessionInfo)
            : base(connectionStringResolver, sessionInfo)
        {
            IocResolver = iocResolver;
            TransactionStrategy = transactionStrategy;
            DbContextResolver = dbContextResolver;

            ActiveDbContexts = new Dictionary<string, EfCoreDbContextBase>();
        }

        protected override void BeginUow()
        {
            if (Options.IsTransactional == true)
            {
                TransactionStrategy.InitOptions(Options);
            }
        }

        protected override void CompleteUow()
        {
            SaveChanges();
            CommitTransaction();
        }

        protected override async Task CompleteUowAsync()
        {
            await SaveChangesAsync();
            CommitTransaction();
        }

        protected override void DisposeUow()
        {
            if (Options.IsTransactional == true)
            {
                TransactionStrategy.Dispose(IocResolver);
            }
            else
            {
                foreach (var context in GetAllActiveDbContexts())
                {
                    context.Dispose();
                    IocResolver.Release(context);
                }
            }

            ActiveDbContexts.Clear();
        }

        private void CommitTransaction()
        {
            if (Options.IsTransactional == true)
            {
                TransactionStrategy.Commit();
            }
        }

        private void SaveChanges()
        {
            foreach (var dbContext in GetAllActiveDbContexts())
            {
                dbContext.SaveChanges();
            }
        }

        private async Task SaveChangesAsync()
        {
            foreach (var dbContext in GetAllActiveDbContexts())
            {
                await dbContext.SaveChangesAsync();
            }
        }

        protected virtual IReadOnlyList<EfCoreDbContextBase> GetAllActiveDbContexts()
        {
            return ActiveDbContexts.Values.ToImmutableList();
        }

        public virtual TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext : EfCoreDbContextBase
        {
            var concreteDbContextType = typeof(TDbContext);

            var connectionString = base.ConnectionStringResolver.GetNameOrConnectionString();

            var dbContextKey = concreteDbContextType.FullName + "#" + connectionString;

            EfCoreDbContextBase dbContext;
            if (!ActiveDbContexts.TryGetValue(dbContextKey, out dbContext))
            {
                if (Options.IsTransactional == true)
                {
                    dbContext = TransactionStrategy.CreateDbContext<TDbContext>(connectionString, DbContextResolver);
                }
                else
                {
                    dbContext = DbContextResolver.Resolve<TDbContext>(connectionString, null);
                }

                ActiveDbContexts[dbContextKey] = dbContext;
            }

            return (TDbContext)dbContext;
        }
    }
}
