using System;
using Transversal.Common.InversionOfControl;
using Transversal.Common.InversionOfControl.Registrar;

namespace Transversal.Core
{
    /// <summary>
    /// Main class that is responsible to bootstrap the entire system.
    /// <para>Prepares dependency injection and registers core components needed for startup.</para>
    /// <para>It must be instantiated and initialized (see <see cref="Bootstrap"/>) first in an application.</para>
    /// </summary>
    public interface IBootstrapper : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="IIoCRegistrarCatalog"/> containing all the application <see cref="IIoCRegistrar"/>
        /// </summary>
        /// <returns><see cref="IIoCRegistrarCatalog"/> instance</returns>
        IIoCRegistrarCatalog GetIoCRegistrarCatalog();

        /// <summary>
        /// Configures the application Inversion Of Control
        /// </summary>
        /// <param name="ioCManager"><see cref="IIoCManager"/> instance</param>
        void ConfigureIoC(IIoCManager ioCManager);
        
        /// <summary>
        /// Runs the bootstrapper
        /// </summary>
        void Bootstrap();
    }
}
