using Microsoft.Extensions.Logging;

namespace Transversal.Common.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILoggerManager"/> that does nothing.
    /// <para>This implementation is useful when the application does not need logging
    /// but there are infrastructure pieces that assume there is a logger.</para>
    /// </summary>
    public class NullLoggerManager : ILoggerManager
    {
        public ILogger<T> CreateLogger<T>()
        {
            return new NullLogger<T>();
        }

        public void Initialize()
        {
        }

        public void Shutdown()
        {
        }
    }
}
