using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Services.Identity.STS.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transversal.Data.EFCore.DbSeeder;

namespace Services.Identity.STS.Core.Data
{
    public class DefaultDbSeeder : IEfCoreDbSeeder<DefaultDbContext>
    {
        private readonly ILogger<IEfCoreDbSeeder<DefaultDbContext>> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;

        public DefaultDbSeeder(ILogger<IEfCoreDbSeeder<DefaultDbContext>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task SeedAsync(DefaultDbContext context, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                if (!context.Users.Any())
                {
                    await context.Users.AddRangeAsync(GetDefaultUsers());
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

        private IEnumerable<User> GetDefaultUsers()
        {
            var user =
            new User()
            {
                UserName = "user@example.com",
                LastName = "User Last Name",
                Name = "User First Name"
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");

            return new List<User>()
            {
                user
            };
        }
    }
}
