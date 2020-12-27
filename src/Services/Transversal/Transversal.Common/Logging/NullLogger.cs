using Microsoft.Extensions.Logging;
using System;

namespace Transversal.Common.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILogger{TCategoryName}"/> that does nothing.
    /// <para>This implementation is useful when the application does not need logging
    /// but there are infrastructure pieces that assume there is a logger.</para>
    /// </summary>
    public class NullLogger<TCatagoryName> : ILogger<TCatagoryName>
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
