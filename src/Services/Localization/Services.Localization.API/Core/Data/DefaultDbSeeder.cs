using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transversal.Data.EFCore.DbSeeder;
using Transversal.Domain.ValueObjects.Localization;

namespace Services.Localization.API.Core.Data
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
                    for (int i = 0; i < 200; i++)
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

        private Domain.Resources.ResourceGroup GenerateResourceGroup(int rgIndex)
        {
            var resourceGroup = new Domain.Resources.ResourceGroup()
            {
                Name = $"Seeded Resource Group {rgIndex}",
                Description = $"This Resource Group has been auto-generated - [Index: {rgIndex}]",
                IsPrivate = new Random().Next(100) <= 50 ? true : false,
                Resources = new List<Domain.Resources.Resource>()
            };

            for (int i = 0; i < new Random().Next(1, 20); i++)
            {
                var resource = new Domain.Resources.Resource()
                {
                    Key = $"RG_{rgIndex}_{i}",
                    Description = $"This Resource has been auto-generated - [Index: {i}]",
                    Value = new LocalizedValueObject()
                };
                resource.Value.SetValue($"RG_{rgIndex}_{i} - EN Value", Transversal.Common.Localization.SupportedLanguages.Codes.en);
                resource.Value.SetValue($"RG_{rgIndex}_{i} - PT Value", Transversal.Common.Localization.SupportedLanguages.Codes.pt);
                resource.Value.SetValue($"RG_{rgIndex}_{i} - ES Value", Transversal.Common.Localization.SupportedLanguages.Codes.es);
                resource.Value.SetValue($"RG_{rgIndex}_{i} - FR Value", Transversal.Common.Localization.SupportedLanguages.Codes.fr);
                resourceGroup.Resources.Add(resource);
            }
            
            return resourceGroup;
        }
    }
}
