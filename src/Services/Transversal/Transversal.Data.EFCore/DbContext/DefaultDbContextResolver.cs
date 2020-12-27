using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;
using Transversal.Common.Exceptions;
using Transversal.Common.InversionOfControl;

namespace Transversal.Data.EFCore.DbContext
{
    public class DefaultDbContextResolver : IDbContextResolver
    {
        public const string DefaultDbContextOptionsParameterName = "options";

        private readonly IIoCResolver _iocResolver;

        public DefaultDbContextResolver(
            IIoCResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public TDbContext Resolve<TDbContext>(DbConnection existingConnection = null)
            where TDbContext : EfCoreDbContextBase
        {
            return _iocResolver.Resolve<TDbContext>(new Dictionary<string, object>()
            {
                { DefaultDbContextOptionsParameterName, CreateOptions<TDbContext>(existingConnection) }
            });
        }

        protected virtual DbContextOptions<TDbContext> CreateOptions<TDbContext>(DbConnection existingConnection = null)
            where TDbContext : EfCoreDbContextBase
        {
            if (_iocResolver.IsRegistered<DbContextOptions<TDbContext>>())
            {
                var dbContextOptionsResolver = _iocResolver.Resolve<IDbContextOptionsResolver<TDbContext>>();
                return dbContextOptionsResolver.GetDbContextOptions(existingConnection);
            }

            throw new DefaultException($"Couldn't resolve DbContextOptions for {typeof(TDbContext).AssemblyQualifiedName}.");
        }
    }
}
