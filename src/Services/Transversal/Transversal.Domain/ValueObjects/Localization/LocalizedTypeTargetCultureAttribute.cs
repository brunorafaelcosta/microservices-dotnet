using System;

namespace Transversal.Domain.ValueObjects.Localization
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LocalizedValueObjectLanguageAttribute : Attribute
    {
        public string Language { get; set; }

        public LocalizedValueObjectLanguageAttribute(string language)
        {
            this.Language = language ?? throw new ArgumentNullException(nameof(language));
        }
    }
}
