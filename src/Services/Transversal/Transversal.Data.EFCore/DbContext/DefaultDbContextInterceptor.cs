using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DiagnosticAdapter;
using System;
using System.Data.Common;
using System.Linq;

namespace Transversal.Data.EFCore.DbContext
{
    public class DefaultDbContextInterceptor : IDbContextInterceptor
    {
        public Func<string> GetCurrentLanguageCode { get; set; }

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

            if (GetCurrentLanguageCode is null)
                return result;

            string currentLanguageCode = GetCurrentLanguageCode();
            if (string.IsNullOrEmpty(currentLanguageCode))
                return result;

            if (!Common.Localization.SupportedLanguages.ToList.Any(l => l.Code == currentLanguageCode))
                throw new InvalidOperationException($"Unsupported application language. [Code: '{currentLanguageCode}']");

            var targetLanguageCode = Common.Localization.SupportedLanguages.ToList.Single(l => l.Code == currentLanguageCode).Code;
            var search = "_" + nameof(Domain.ValueObjects.Localization.LocalizedValueObject.InvariantValue);
            var replace = "_" + string.Format(Domain.ValueObjects.Localization.LocalizedValueObject.ValuePropertyNameFormat, targetLanguageCode.ToUpperInvariant());

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
