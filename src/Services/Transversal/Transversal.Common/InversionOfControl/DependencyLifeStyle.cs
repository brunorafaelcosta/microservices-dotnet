namespace Transversal.Common.InversionOfControl
{
    /// <summary>
    /// Lifestyles of types used in dependency injection.
    /// </summary>
    public enum DependencyLifeStyle
    {
        /// <summary>
        /// Singleton object. Created a single object on first resolving
        /// and same instance is used for subsequent resolves
        /// </summary>
        Singleton,

        /// <summary>
        /// Transient object. Created one object for every resolving
        /// </summary>
        Transient,

        /// <summary>
        /// Some application types naturally lend themselves to "request" type semantics, for example ASP.NET web forms and MVC applications.
        /// <para>In these application types, it’s helpful to have the ability to have a sort of "singleton per request"</para>
        /// </summary>
        SingletonPerRequest
    }
}
