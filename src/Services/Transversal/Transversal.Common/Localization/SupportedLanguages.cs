using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Transversal.Common.Localization
{
    /// <summary>
    /// Application supported languages
    /// </summary>
    public static class SupportedLanguages
    {
        public static class Codes
        {
            public const string en = "en";
            public const string pt = "pt";
            public const string es = "es";
            public const string fr = "fr";
        }

        private static Language en => new Language(Codes.en) { Name = "English (United States of America)", IsDefault = true };
        private static Language pt => new Language(Codes.pt) { Name = "Português (Portugal)" };
        private static Language es => new Language(Codes.es) { Name = "Español (España)" };
        private static Language fr => new Language(Codes.fr) { Name = "Français (France)" };

        public static List<Language> ToList => new List<Language>()
        {
            en,
            pt,
            es,
            fr
        };

        public static List<CultureInfo> CultureInfoToList => ToList.Select(l => l.CultureInfo).ToList();

        public static Language Default => ToList.Single(c => c.IsDefault);
    }
}
