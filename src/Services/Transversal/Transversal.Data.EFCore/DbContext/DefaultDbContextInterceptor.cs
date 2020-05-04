using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DiagnosticAdapter;
using System;
using System.Data.Common;
using System.Linq;

namespace Transversal.Data.EFCore.DbContext
{
    public class DefaultDbContextInterceptor : IDbContextInterceptor
    {
        public string LanguageCode { get; set; }

        public DefaultDbContextInterceptor()
        {
        }

        [DiagnosticName("Microsoft.EntityFrameworkCore.Database.Command.CommandExecuting")]
        public void OnCommandExecuting(DbCommand command, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, bool async, DateTimeOffset startTime)
        {
            if (command != null)
            {
                command.CommandText = ApplyLocalization(command.CommandText);
            }
        }

        private string ApplyLocalization(string commandText)
        {
            string result = commandText;

            if (string.IsNullOrEmpty(LanguageCode))
                return result;

            var targetCultureCode = Common.Localization.SupportedLanguages.ToList.Single(l => l.Code == LanguageCode).Code;
            var search = "_" + nameof(Domain.ValueObjects.Localization.LocalizedValueObject.InvariantValue);
            var replace = "_" + string.Format(Domain.ValueObjects.Localization.LocalizedValueObject.ValuePropertyNameFormat, targetCultureCode.ToUpperInvariant());

            if (!string.IsNullOrEmpty(commandText) &&
                commandText.Contains(search) &&
                !commandText.Contains("UPDATE") &&
                !commandText.Contains("INSERT"))
            {
                result = commandText.Replace(search, replace);
            }

            return result;
        }
    }
}
