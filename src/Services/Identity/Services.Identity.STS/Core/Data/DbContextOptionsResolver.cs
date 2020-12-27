using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data.Common;
using System.Reflection;
using Transversal.Data.EFCore.DbContext;

namespace Services.Identity.STS.Core.Data
{
    public class DbContextOptionsResolver<TDbContext> : IDbContextOptionsResolver<TDbContext>
        where TDbContext : EfCoreDbContextBase
    {
        private readonly IConnectionStringResolver _connectionStringResolver;

        public DbContextOptionsResolver(IConnectionStringResolver connectionStringResolver)
        {
            _connectionStringResolver = connectionStringResolver ?? throw new ArgumentNullException(nameof(connectionStringResolver));
        }

        public DbContextOptions<TDbContext> GetDbContextOptions(DbConnection existingConnection = null)
        {
            var connectionString = _connectionStringResolver.GetNameOrConnectionString<TDbContext>();

            var builder = new DbContextOptionsBuilder<TDbContext>();

#if DEBUG
            builder
                .LogTo(str => Console.WriteLine(str), Microsoft.Extensions.Logging.LogLevel.Debug)
                .EnableSensitiveDataLogging();
#endif

            static void BuildSqlServerOptions(SqlServerDbContextOptionsBuilder sqlServerOptionsBuilder)
            {
                sqlServerOptionsBuilder.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
            }

            if (existingConnection is null)
            {
                builder.UseSqlServer(connectionString, BuildSqlServerOptions);
            }
            else
            {
                builder.UseSqlServer(existingConnection, BuildSqlServerOptions);
            }

            return builder.Options;
        }
    }
}
