using System;

namespace Transversal.Domain.ValueObjects.Localization
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LocalizedValueObjectLanguageAttribute : Attribute
    {
        public string LanguageCode { get; set; }

        public LocalizedValueObjectLanguageAttribute(string languageCode)
        {
            this.LanguageCode = languageCode ?? throw new ArgumentNullException(nameof(languageCode));
        }
    }
}
