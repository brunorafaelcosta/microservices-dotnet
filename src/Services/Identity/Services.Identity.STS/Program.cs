using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using System;
using System.IO;
using System.Net;
using Transversal.Data.EFCore.DbMigrator;

namespace Services.Identity.STS
{
    public class Program
    {
        protected static string ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace;

        public static void Main(string[] args)
        {
            IConfiguration configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = CreateHostBuilder(args, configuration).Build();

                Log.Information("Applying migrations ({ApplicationContext})...", AppName);
                MigrateDbContexts(host, Log.Logger);

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(options => SetKestrelConfiguration(options, configuration))
                        .UseStartup<Startup>()
                        .UseConfiguration(configuration);
                })
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ASPNETCORE_ENVIRONMENT}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        private static void SetKestrelConfiguration(KestrelServerOptions options, IConfiguration configuration)
        {
            var portsConfig = configuration.GetSection("Ports");
            int httpPort = portsConfig.GetValue<int>("Http");

            options.Listen(IPAddress.Any, httpPort, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            });
        }

        private static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.WithProperty("CorrelationId", Guid.NewGuid().ToString())
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .ReadFrom.Configuration(configuration, "Logger")
                .CreateLogger();
        }

        private static void MigrateDbContexts(IHost host, ILogger logger)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var defaultDbMigrator = services.GetService<IEFCoreDbMigrator<Core.Data.DefaultDbContext>>();
                if (defaultDbMigrator != null)
                {
                    defaultDbMigrator.CreateOrMigrate();
                }
            }
        }
    }
}
