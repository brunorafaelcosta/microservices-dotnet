using Transversal.Core;

namespace Transversal.Web
{
    public abstract class WebBootstrapperSettingsBase : BootstrapperSettingsBase, IBootstrapperSettings
    {
        #region ASPNet

        public class ASPNetSettings
        {
            public string PathBase { get; set; }

            public bool UseCorsPolicy { get; set; }
            public string CorsPolicyName { get; set; }
        }

        public virtual ASPNetSettings ASPNet { get; set; }

        #endregion

        protected override void Defaults()
        {
            base.Defaults();

            ASPNet = new ASPNetSettings
            {
                PathBase = null,
                UseCorsPolicy = false
            };
        }
    }
}
