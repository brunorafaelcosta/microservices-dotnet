using System;

namespace Transversal.Common.Session
{
    /// <summary>
    /// Implementation of <see cref="ISessionInfo"/> that does nothing
    /// <para>This implementation is useful when the application does not need sessions
    /// but there are infrastructure pieces that assume there is a session</para>
    /// </summary>
    public class NullSessionInfo : ISessionInfo
    {
        public long? UserId => null;

        public int? TenantId => null;

        public string Name => null;

        public string LanguageCode => null;

        public DateTime? StartTime => null;
    }
}
