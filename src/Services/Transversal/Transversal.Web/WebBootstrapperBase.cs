using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using System;
using Transversal.Common.InversionOfControl;
using Transversal.Core;
using Transversal.Web.InversionOfControl;

namespace Transversal.Web
{
    public abstract class WebBootstrapperBase<TWebBootstrapperSettings> : BootstrapperBase
        where TWebBootstrapperSettings : WebBootstrapperSettingsBase, IBootstrapperSettings, new()
    {
        #region Fields

        protected readonly IConfiguration _configuration;
        protected readonly IWebHostEnvironment _webHostEnvironment;

        protected TWebBootstrapperSettings _webBootstrapperSettings => ((TWebBootstrapperSettings)_bootstrapperSettings);

        #endregion Fields

        #region Ctor

        public WebBootstrapperBase(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
            : base()
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));

            #region Bootstrapper settings
            _bootstrapperSettings = new TWebBootstrapperSettings();
            var bootstrapperConfigurationSection = _configuration.GetSection(_webBootstrapperSettings.ConfigurationSectionName);
            bootstrapperConfigurationSection.Bind(_bootstrapperSettings);
            #endregion
        }

        #endregion Ctor

        #region Methods

        #region Configure services

        public virtual void ConfigureServiceCollection(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            AddCorsToServiceCollection(serviceCollection);

            AddAuthenticationToServiceCollecion(serviceCollection);
        }

        protected virtual void AddCorsToServiceCollection(IServiceCollection serviceCollection)
        {
            if (_webBootstrapperSettings.ASPNet.UseCorsPolicy)
            {
                serviceCollection.AddCors(options =>
                {
                    options.AddPolicy(_webBootstrapperSettings.ASPNet.CorsPolicyName ?? options.DefaultPolicyName,
                        builder => builder
                            .SetIsOriginAllowed((host) => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
                });
            }
        }

        protected virtual void AddAuthenticationToServiceCollecion(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient<Common.Session.ISessionInfo, Session.SessionInfo>();
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            var autofacIoCManager = new AutofacIoCManager(builder);

            autofacIoCManager.RegisterInstance((TWebBootstrapperSettings)_bootstrapperSettings);

            autofacIoCManager.RegisterIfNot<Common.Logging.ILoggerManager, Logging.SerilogLoggerManager>(DependencyLifeStyle.SingletonPerRequest);

            ConfigureIoC(autofacIoCManager);
        }

        #endregion

        #region Configure application

        public virtual void ConfigureApplication(IApplicationBuilder app, Action<IEndpointRouteBuilder> onEndpointRouteBuilder = null)
        {
            Bootstrap();

            ConfigurePathBase(app);

            ConfigureCors(app);
        }

        protected virtual void ConfigurePathBase(IApplicationBuilder app)
        {
            if (!string.IsNullOrEmpty(_webBootstrapperSettings.ASPNet.PathBase))
            {
                _logger.LogDebug($"Configuring Path Base '{_webBootstrapperSettings.ASPNet.PathBase}'...");

                app.UsePathBase(_webBootstrapperSettings.ASPNet.PathBase);
            }
        }

        protected virtual void ConfigureCors(IApplicationBuilder app)
        {
            if (_webBootstrapperSettings.ASPNet.UseCorsPolicy)
            {
                _logger.LogDebug($"Configuring Cors Policy...");

                if (!string.IsNullOrEmpty(_webBootstrapperSettings.ASPNet.CorsPolicyName))
                    app.UseCors(_webBootstrapperSettings.ASPNet.CorsPolicyName);
                else
                    app.UseCors();
            }
        }

        #endregion

        #endregion Methods
    }
}
