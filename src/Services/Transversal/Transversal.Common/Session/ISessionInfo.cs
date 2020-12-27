using System;

namespace Transversal.Common.Session
{
    /// <summary>
    /// Defines the current session information.
    /// </summary>
    public interface ISessionInfo
    {
        /// <summary>
        /// Gets current UserId or null
        /// </summary>
        /// <remarks>
        /// It can be null if no user is logged in
        /// </remarks>
        long? UserId { get; }

        /// <summary>
        /// Gets current TenantId or null
        /// <para>It should be the TenantId of the <see cref="UserId"/></para>
        /// </summary>
        /// <remarks>
        /// It can be null if no user is logged in
        /// </remarks>
        int? TenantId { get; }

        /// <summary>
        /// Gets current Name or null
        /// </summary>
        /// <remarks>
        /// It can be null if no user is logged in
        /// </remarks>
        string Name { get; }

        /// <summary>
        /// Gets current user language code or null
        /// </summary>
        /// <remarks>
        /// It can be null if no user is logged in
        /// </remarks>
        string LanguageCode { get; }

        /// <summary>
        /// Gets current Start Time or null
        /// </summary>
        /// <remarks>
        /// It can be null if no user is logged in
        /// </remarks>
        DateTime? StartTime { get; }
    }
}