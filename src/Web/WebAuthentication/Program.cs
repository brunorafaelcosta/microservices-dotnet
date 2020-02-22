using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Http.BatchFormatters;

namespace WebAuthentication
{
    public class Program
    {
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
                        .UseStartup<Startup>()
                        .UseConfiguration(configuration);
                })
                .UseSerilog();
        
        private static IConfiguration GetConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            return builder.Build();
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var logstashUri = Convert.ToString(configuration["Logger:Uri"]);
            var bufferBaseFileName = Convert.ToString(configuration["Logger:BufferBaseFileName"]);
            var retainedBufferFileCountLimit = Convert.ToInt32(configuration["Logger:RetainedBufferFileCountLimit"]);
            var bufferFileSizeLimitBytes = Convert.ToInt64(configuration["Logger:BufferFileSizeLimitBytes"]);
            
            return new LoggerConfiguration()
                .Enrich.WithProperty("ApplicationContext", Namespace)
                .Enrich.WithProperty("CorrelationId", Guid.NewGuid().ToString())
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Information()
                .WriteTo.DurableHttpUsingFileSizeRolledBuffers(
                    requestUri: logstashUri,
                    batchFormatter: new ArrayBatchFormatter(),
                    textFormatter: new ElasticsearchJsonFormatter(),
                    bufferBaseFileName: bufferBaseFileName,
                    retainedBufferFileCountLimit: retainedBufferFileCountLimit,
                    bufferFileSizeLimitBytes: bufferFileSizeLimitBytes
                )
                .CreateLogger();
        }
    }
}
