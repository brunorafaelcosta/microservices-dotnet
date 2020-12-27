using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Transversal.Web
{
    /// <summary>
    /// Provides additional methods to <see cref="IHostBuilder"/>.
    /// </summary>
    public static class WebProgramExtensions
    {
        public static IHostBuilder SetServiceProviderFactory(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        }

        public static IHostBuilder SetLogger(this IHostBuilder hostBuilder, IConfiguration loggerConfiguration
            , string applicationContext, string correlationId = null)
        {
            if (loggerConfiguration is null)
                return hostBuilder;

            Log.Logger = Logging.SerilogExtensions.CreateSerilogLogger(loggerConfiguration, applicationContext, correlationId);

            return hostBuilder.UseSerilog();
        }
    }
}
