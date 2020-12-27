using Microsoft.Extensions.Logging;
using Transversal.Common.Logging;

namespace Transversal.Web.Logging
{
    public class SerilogLoggerManager : ILoggerManager
    {
        private readonly ILoggerFactory _loggerFactory;

        public SerilogLoggerManager(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public ILogger<T> CreateLogger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }

        public void Initialize()
        {
            // Do nothing because the Serilog logger is managed by the Program Host
        }

        public void Shutdown()
        {
        }
    }
}
