using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Transversal.Web;
using Transversal.Web.API;

namespace Services.Resources.API
{
    public class Program : APIProgramBase<Program>
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args, GetConfiguration())
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(options => SetKestrelConfiguration(options, configuration.GetSection(KestrelConfigurationSectionName)))
                        .UseStartup<Startup>()
                        .UseConfiguration(configuration);
                })
                .SetLogger(configuration.GetSection(LoggerConfigurationSectionName), AppName)
                .SetServiceProviderFactory();
    }
}
