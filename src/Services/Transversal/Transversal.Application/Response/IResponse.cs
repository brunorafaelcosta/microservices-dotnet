namespace Transversal.Application.Response
{
    /// <summary>
    /// Response parameters used by <see cref="IApplicationService"/> methods.
    /// </summary>
    /// <typeparam name="T">Result type</typeparam>
    public interface IResponse<T>
    {
        /// <summary>
        /// Response result
        /// </summary>
        T Result { get; }
    }
}
