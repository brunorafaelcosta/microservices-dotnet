using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Transversal.Common.InversionOfControl.Registrar;
using Transversal.Web.API;

namespace Services.Resources.API
{
    public class Startup : APIBootstrapperBase<Settings>
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
            : base(configuration, env)
        {
        }

        public override IIoCRegistrarCatalog GetIoCRegistrarCatalog()
        {
            var catalog = new IoCRegistrarCatalog();
            catalog.Register<Core.Data.DataIoCRegistrar>();
            catalog.Register<Core.Application.ApplicationIoCRegistrar>();
            return catalog;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServiceCollection(services);

            AddGrpcToServiceCollection(services);
        }
        private void AddGrpcToServiceCollection(IServiceCollection services)
        {
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            ConfigureApplication(app, ConfigureEndpoints);

            app.UseSerilogRequestLogging();
        }
        private void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            ConfigureGrpcEndpoints(endpoints);
        }
        private void ConfigureGrpcEndpoints(IEndpointRouteBuilder endpoints)
        {
            _logger.LogDebug("Configuring Grpc...");

            endpoints.MapGet("/_proto", async ctx =>
            {
                ctx.Response.ContentType = "text/plain";

                var response = new System.Text.StringBuilder();

                var protoFiles = System.IO.Directory.EnumerateFiles(
                    System.IO.Path.Combine(_webHostEnvironment.ContentRootPath, "Grpc"),
                    "*.proto",
                    System.IO.SearchOption.AllDirectories);

                foreach (string protoFile in protoFiles)
                {
                    var protoFileInfo = new System.IO.FileInfo(protoFile);

                    response.AppendLine(protoFileInfo.Name);

                    using var fs = new System.IO.FileStream(protoFileInfo.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    using var sr = new System.IO.StreamReader(fs);
                    while (!sr.EndOfStream)
                    {
                        var line = await sr.ReadLineAsync();
                        if (line != "/* >>" || line != "<< */")
                        {
                            response.AppendLine(line);
                        }
                    }

                    response.AppendLine();
                }

                await ctx.Response.WriteAsync(response.ToString());
            });

            endpoints.MapGrpcService<Grpc.Implementations.ResourceService>();
        }

        protected override void OnBootstrapCompleted()
        {
            base.OnBootstrapCompleted();

            var defaultDbMigrator = _ioCManager.Resolve<Transversal.Data.EFCore.DbMigrator.IEFCoreDbMigrator<Core.Data.DefaultDbContext>>();
            if (defaultDbMigrator != null)
            {
                defaultDbMigrator.CreateOrMigrate();
            }
        }
    }
}
