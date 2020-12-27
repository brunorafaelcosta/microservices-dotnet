using Microsoft.Extensions.Logging;

namespace Transversal.Common.Logging
{
    /// <summary>
    /// Interface that will configure the <see cref="ILogger"/>.
    /// </summary>
    public interface ILoggerManager
    {
        /// <summary>
        /// Initializes the <see cref="ILogger"/>
        /// </summary>
        void Initialize();

        /// <summary>
        /// Shuts down the <see cref="ILogger"/>
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Creates a new <see cref="ILogger"/> instance using the full name of the given type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns><see cref="ILogger"/> instance</returns>
        ILogger<T> CreateLogger<T>();
    }
}
