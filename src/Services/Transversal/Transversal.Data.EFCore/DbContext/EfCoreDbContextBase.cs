using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transversal.Common.Extensions;

namespace Transversal.Data.EFCore.DbContext
{
    /// <summary>
    /// Base class for all DbContexts in the application.
    /// </summary>
    public abstract class EfCoreDbContextBase : Microsoft.EntityFrameworkCore.DbContext
    {
        protected virtual IDbContextInterceptor Interceptor { get; set; }

        protected EfCoreDbContextBase(DbContextOptions options, IDbContextInterceptor interceptor)
            : base(options)
        {
            InitializeDbContext(interceptor);
        }

        protected virtual void InitializeDbContext(IDbContextInterceptor interceptor)
        {
            this.Interceptor = interceptor;
            var listener = this.GetService<DiagnosticSource>().As<DiagnosticListener>();
            listener.SubscribeWithAdapter(this.Interceptor);
        }

        public override int SaveChanges()
        {
            try
            {
                var result = base.SaveChanges();
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Owned<Domain.ValueObjects.Localization.LocalizedValueObject>();
        }
        
        public void OverrideLanguage(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode) ||
                !Common.Localization.SupportedLanguages.ToList.Any(l => l.Code == languageCode))
                throw new InvalidOperationException($"Unsupported application language. [Code: '{languageCode}']");

            if (Interceptor != null)
            {
                Interceptor.LanguageCode = languageCode;
            }
        }
    }
}
