using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Transversal.Data.EFCore.DbSeeder;

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
                    await context.ResourceGroups.AddAsync(new Domain.Resources.ResourceGroup()
                    {
                        Name = "Seeded Resource Group 1",
                        Description = "Seeded Resource Group one",
                        IsPrivate = true
                    });
                    await context.ResourceGroups.AddAsync(new Domain.Resources.ResourceGroup()
                    {
                        Name = "Seeded Resource Group 2",
                        Description = "Seeded Resource Group two",
                        IsPrivate = true
                    });
                    await context.ResourceGroups.AddAsync(new Domain.Resources.ResourceGroup()
                    {
                        Name = "Seeded Resource Group 3",
                        Description = "Seeded Resource Group three",
                        IsPrivate = true
                    });
                    await context.ResourceGroups.AddAsync(new Domain.Resources.ResourceGroup()
                    {
                        Name = "Seeded Resource Group 4",
                        Description = "Seeded Resource Group four",
                        IsPrivate = true
                    });
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
    }
}
