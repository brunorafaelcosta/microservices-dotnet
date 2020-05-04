using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Data.Common;

namespace Transversal.Data.EFCore.DbContext
{
    /// <summary>
    /// DbContext queries interceptor.
    /// <para>Mainly used for for localization purposes.</para>
    /// </summary>
    public interface IDbContextInterceptor
    {
        /// <summary>
        /// Language to be used on the executing command
        /// </summary>
        string LanguageCode { get; set; }

        void OnCommandExecuting(DbCommand command, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, bool async, DateTimeOffset startTime);
    }
}
