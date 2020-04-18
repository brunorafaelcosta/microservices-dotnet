using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;
using Transversal.Common.Exceptions;
using Transversal.Common.Dependency;

namespace Transversal.Data.EFCore
{
    public class DefaultDbContextResolver : IDbContextResolver
    {
        private readonly IIocResolver _iocResolver;

        public DefaultDbContextResolver(
            IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public TDbContext Resolve<TDbContext>(string connectionString, DbConnection existingConnection)
            where TDbContext : DbContextBase
        {
            return _iocResolver.Resolve<TDbContext>(new Dictionary<string, object>()
            {
                { "options", CreateOptions<TDbContext>(connectionString, existingConnection) }
            });
        }

        protected virtual DbContextOptions<TDbContext> CreateOptions<TDbContext>(string connectionString, DbConnection existingConnection = null)
            where TDbContext : DbContextBase
        {
            if (_iocResolver.IsRegistered<DbContextOptions<TDbContext>>())
            {
                return _iocResolver.Resolve<DbContextOptions<TDbContext>>();
            }

            throw new DefaultException($"Couldn't resolve DbContextOptions for {typeof(TDbContext).AssemblyQualifiedName}.");
        }
    }
}
