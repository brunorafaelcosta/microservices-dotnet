using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Transversal.Web
{
    public abstract class WebProgramBase<TProgram>
        where TProgram : class, new()
    {
        #region Static fields

        protected static string ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        protected static readonly string Namespace = typeof(TProgram).Namespace;
        protected static readonly string AppName = Namespace;

        protected static readonly string KestrelConfigurationSectionName = "Kestrel";
        protected static readonly string LoggerConfigurationSectionName = "Logger";

        #endregion Static fields

        #region Static methods

        protected static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ASPNETCORE_ENVIRONMENT}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        protected static void SetKestrelConfiguration(KestrelServerOptions options, IConfiguration kestrelConfiguration)
        {
            if (kestrelConfiguration is null)
                return;

            var portsConfig = kestrelConfiguration.GetSection("Ports")?.Get<Dictionary<string, Dictionary<string, string>>>();
            if (portsConfig is null)
                return;

            foreach (var portConfig in portsConfig)
            {
                var portConfigValues = portConfig.Value;

                if (!int.TryParse(portConfigValues["Port"], out int number))
                    continue;
                if (!Enum.TryParse(portConfigValues["Protocols"], out HttpProtocols protocols))
                    continue;

                options.Listen(IPAddress.Any, number, listenOptions =>
                {
                    listenOptions.Protocols = protocols;
                });
            }
        }

        #endregion Static methods
    }
}
