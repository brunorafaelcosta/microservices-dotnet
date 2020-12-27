using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using System;

namespace Transversal.Web.Logging
{
    /// <summary>
    /// Provides additional methods to <see cref="Serilog"/>.
    /// </summary>
    public static class SerilogExtensions
    {
        public static ILogger CreateSerilogLogger(IConfiguration loggerConfiguration, string applicationContext, string correlationId = null)
        {
            return new LoggerConfiguration()
                .Enrich.WithProperty("ApplicationContext", applicationContext)
                .Enrich.WithProperty("CorrelationId", correlationId ?? Guid.NewGuid().ToString())
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .ReadFrom.Configuration(loggerConfiguration)
                .CreateLogger();
        }
    }
}
