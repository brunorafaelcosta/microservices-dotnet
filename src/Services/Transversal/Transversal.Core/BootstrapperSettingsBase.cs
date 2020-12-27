namespace Transversal.Core
{
    public class BootstrapperSettingsBase : IBootstrapperSettings
    {
        public virtual string ConfigurationSectionName => "Bootstrapper";

        public BootstrapperSettingsBase()
        {
            Defaults();
        }

        protected virtual void Defaults()
        {
        }
    }
}
