using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Transversal.Common.Localization;

namespace Transversal.Domain.ValueObjects.Localization
{
    public class LocalizedValueObject : ValueObject
    {
        #region Language Values

        public const string ValuePropertyNameFormat = "{0}Value";

		[LocalizedValueObjectLanguage(SupportedLanguages.Codes.en)]
		public string ENValue { get; set; }

        [LocalizedValueObjectLanguage(SupportedLanguages.Codes.pt)]
        public string PTValue { get; set; }

        [LocalizedValueObjectLanguage(SupportedLanguages.Codes.es)]
        public string ESValue { get; set; }

        [LocalizedValueObjectLanguage(SupportedLanguages.Codes.fr)]
        public string FRValue { get; set; }
        
        #endregion Language Values

        public string InvariantValue { get; private set; }

        public void SetValue(string value, string languageCode)
        {
            var targetLanguage = SupportedLanguages.ToList.Single(l => l.Code == languageCode);
            var targetProperty = GetType()
                .GetProperties()
                .Where(x => x.GetCustomAttributes<LocalizedValueObjectLanguageAttribute>()
                    .Any(l => l.LanguageCode.ToUpperInvariant() == targetLanguage.Code.ToUpperInvariant()))
                .FirstOrDefault();

            if (targetProperty is null)
                return;

            targetProperty.SetValue(this, value);

            if (string.IsNullOrEmpty(InvariantValue))
                InvariantValue = value;
        }

        public string GetValue(string languageCode)
        {
            var targetLanguage = SupportedLanguages.ToList.Single(l => l.Code == languageCode);
            PropertyInfo targetProperty = GetType()
                .GetProperties()
                .Where(x => x.GetCustomAttributes<LocalizedValueObjectLanguageAttribute>()
                    .Any(l => l.LanguageCode.ToUpperInvariant() == targetLanguage.Code.ToUpperInvariant()))
                .FirstOrDefault();
            
            if (targetProperty is null)
                return InvariantValue;

            string value = (string)targetProperty.GetValue(this);

            return value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return InvariantValue;
            yield return ENValue;
            yield return PTValue;
            yield return ESValue;
            yield return FRValue;
        }

        public override string ToString()
        {
            return GetValue(SupportedLanguages.Default.Code) ?? InvariantValue;
        }
    }
}
