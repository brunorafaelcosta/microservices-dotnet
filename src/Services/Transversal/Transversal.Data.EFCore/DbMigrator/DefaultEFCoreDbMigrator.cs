using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Transactions;
using Transversal.Data.EFCore.DbContext;
using Transversal.Data.EFCore.DbSeeder;
using Transversal.Domain.Uow.Manager;

namespace Transversal.Data.EFCore.DbMigrator
{
    public class DefaultEFCoreDbMigrator<TDbContext> : IEFCoreDbMigrator<TDbContext>
        where TDbContext : EfCoreDbContextBase
    {
        private readonly ILogger<DefaultEFCoreDbMigrator<TDbContext>> _logger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDbContextResolver _dbContextResolver;
        private readonly IEfCoreDbSeeder<TDbContext> _seeder;

        public DefaultEFCoreDbMigrator(
            ILogger<DefaultEFCoreDbMigrator<TDbContext>> logger,
            IUnitOfWorkManager unitOfWorkManager,
            IDbContextResolver dbContextResolver,
            IEfCoreDbSeeder<TDbContext> seeder)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWorkManager = unitOfWorkManager ?? throw new ArgumentNullException(nameof(unitOfWorkManager));
            _dbContextResolver = dbContextResolver ?? throw new ArgumentNullException(nameof(dbContextResolver));
            _seeder = seeder ?? throw new ArgumentNullException(nameof(seeder));
        }

        public virtual void CreateOrMigrate()
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
                {
                    using (var dbContext = _dbContextResolver.Resolve<TDbContext>())
                    {
                        _logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TDbContext).Name);

                        dbContext.Database.EnsureCreated();
                        dbContext.Database.Migrate();

                        if (_seeder != null)
                        {
                            _seeder.SeedAsync(dbContext);
                        }

                        // Explicit call save changes because we aren't using a repository here!
                        dbContext.SaveChanges();

                        uow.Complete();

                        _logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TDbContext).Name);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TDbContext).Name);

                throw ex;
            }
        }
    }
}
