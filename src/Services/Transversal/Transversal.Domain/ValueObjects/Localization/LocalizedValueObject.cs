using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Transversal.Domain.ValueObjects.Localization
{
    public class LocalizedValueObject : ValueObject
    {
        #region Language Values

		[LocalizedValueObjectLanguageAttribute("en")]
		public string Value_en { get; private set; }
		
        [LocalizedValueObjectLanguageAttribute("pt")]
		public string Value_pt { get; private set; }
		
        [LocalizedValueObjectLanguageAttribute("es")]
		public string Value_es { get; private set; }

		[LocalizedValueObjectLanguageAttribute("fr")]
		public string Value_fr { get; private set; }
        
        #endregion Language Values

        public string Value { get; private set; }

        public void SetValue(string value, string language)
        {
            var targetProperty = this.GetType()
                .GetProperties()
                .Where(x => x.GetCustomAttributes<LocalizedValueObjectLanguageAttribute>().Any(l => l.Language == language))
                .FirstOrDefault();

            if (targetProperty is null)
                return;

            targetProperty.SetValue(this, value);
        }

        public string GetValue(string language)
        {
            PropertyInfo targetProperty = this.GetType()
                .GetProperties()
                .Where(x => x.GetCustomAttributes<LocalizedValueObjectLanguageAttribute>().Any(l => l.Language == language))
                .FirstOrDefault();
            
            if (targetProperty is null)
                return null;

            string value = (string)targetProperty.GetValue(this);

            return value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value_en;
            yield return Value_pt;
            yield return Value_es;
            yield return Value_fr;
        }

        public override string ToString()
        {
            return GetValue("en");
        }
    }
}
