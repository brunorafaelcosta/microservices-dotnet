using System.Globalization;

namespace Transversal.Common.Localization
{
    /// <summary>
    /// Defines an application language
    /// </summary>
    public class Language
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public CultureInfo CultureInfo { get; set; }

        public Language(string code)
        {
            this.Code = code ?? throw new System.ArgumentNullException(nameof(code));
            this.IsDefault = false;
        }
    }
}
