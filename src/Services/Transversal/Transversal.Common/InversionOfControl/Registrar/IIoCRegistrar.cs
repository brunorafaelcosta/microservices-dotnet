namespace Transversal.Common.InversionOfControl.Registrar
{
    /// <summary>
    /// Interface that will be implemented by the application Inversion of Control registrars.
    /// </summary>
    public interface IIoCRegistrar
    {
        /// <summary>
        /// Runs the registrar
        /// </summary>
        /// <param name="ioCManager"><see cref="IIoCManager"/> instance</param>
        void Run(IIoCManager ioCManager);
    }
}
