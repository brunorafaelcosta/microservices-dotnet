using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transversal.Data.EFCore.DbSeeder;
using Transversal.Domain.ValueObjects.Localization;

namespace Services.Resources.API.Core.Data
{
    public class DefaultDbSeeder : IEfCoreDbSeeder<DefaultDbContext>
    {
        private readonly ILogger<IEfCoreDbSeeder<DefaultDbContext>> _logger;

        public DefaultDbSeeder(ILogger<IEfCoreDbSeeder<DefaultDbContext>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SeedAsync(DefaultDbContext context, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                if (!context.ResourceGroups.Any())
                {
                    await context.ResourceGroups.AddAsync(GenerateIdentityResourceGroup());

                    for (int i = 0; i < 100; i++)
                    {
                        await context.ResourceGroups.AddAsync(GenerateResourceGroup(i));
                    }
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    _logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(DefaultDbContext));

                    await SeedAsync(context, retryForAvaiability);
                }
            }
        }

        private Domain.ResourceGroup GenerateIdentityResourceGroup()
        {
            var resourceGroup = new Domain.ResourceGroup()
            {
                Code = $"identity_sts",
                Description = $"Public resources for the Identity STS pages",
                IsPrivate = false,
                Resources = new List<Domain.Resource>()
            };

            var loginResource = new Domain.Resource()
            {
                Key = $"login_lb",
                Description = $"Login label",
                Value = new LocalizedValueObject(),
                TenantId = null
            };
            loginResource.Value.SetValue($"Login", Transversal.Common.Localization.SupportedLanguages.Codes.en);
            loginResource.Value.SetValue($"Entrar", Transversal.Common.Localization.SupportedLanguages.Codes.pt);
            loginResource.Value.SetValue($"Iniciar Sesión", Transversal.Common.Localization.SupportedLanguages.Codes.es);
            loginResource.Value.SetValue($"Connexion", Transversal.Common.Localization.SupportedLanguages.Codes.fr);
            resourceGroup.Resources.Add(loginResource);

            return resourceGroup;
        }

        private Domain.ResourceGroup GenerateResourceGroup(int rgIndex)
        {
            var resourceGroup = new Domain.ResourceGroup()
            {
                Code = $"Seeded Resource Group {rgIndex}",
                Description = $"This Resource Group has been auto-generated - [Index: {rgIndex}]",
                IsPrivate = new Random().Next(100) <= 50 ? true : false,
                Resources = new List<Domain.Resource>()
            };

            for (int i = 0; i < new Random().Next(1, 20); i++)
            {
                var resource = new Domain.Resource()
                {
                    Key = $"RG_{rgIndex}_{i}",
                    Description = $"This Resource has been auto-generated - [Index: {i}]",
                    Value = new LocalizedValueObject(),
                    TenantId = null
                };
                resource.Value.SetValue($"RG_{rgIndex}_{i} - EN Value", Transversal.Common.Localization.SupportedLanguages.Codes.en);
                resource.Value.SetValue($"RG_{rgIndex}_{i} - PT Value", Transversal.Common.Localization.SupportedLanguages.Codes.pt);
                resource.Value.SetValue($"RG_{rgIndex}_{i} - ES Value", Transversal.Common.Localization.SupportedLanguages.Codes.es);
                resource.Value.SetValue($"RG_{rgIndex}_{i} - FR Value", Transversal.Common.Localization.SupportedLanguages.Codes.fr);
                resourceGroup.Resources.Add(resource);

                if (resourceGroup.IsPrivate)
                {
                    var overridenEntityResource = new Domain.Resource()
                    {
                        Key = $"RG_{rgIndex}_{i}",
                        Description = $"This Resource has been auto-generated- [Index: {i}] [Overriden for tenant 1]",
                        Value = new LocalizedValueObject(),
                        TenantId = 1,
                        TenantlessEntity = resource
                    };
                    overridenEntityResource.Value.SetValue($"RG_{rgIndex}_{i} - EN Value [Overriden for tenant 1]", Transversal.Common.Localization.SupportedLanguages.Codes.en);
                    overridenEntityResource.Value.SetValue($"RG_{rgIndex}_{i} - PT Value [Overriden for tenant 1]", Transversal.Common.Localization.SupportedLanguages.Codes.pt);
                    overridenEntityResource.Value.SetValue($"RG_{rgIndex}_{i} - ES Value [Overriden for tenant 1]", Transversal.Common.Localization.SupportedLanguages.Codes.es);
                    overridenEntityResource.Value.SetValue($"RG_{rgIndex}_{i} - FR Value [Overriden for tenant 1]", Transversal.Common.Localization.SupportedLanguages.Codes.fr);
                    resourceGroup.Resources.Add(overridenEntityResource);
                }
            }
            
            return resourceGroup;
        }
    }
}
