using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Services.Identity.STS.Application.Models;
using System.Linq;
using System.Collections.Generic;

namespace Services.Identity.STS.Application.Data
{
    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();
        
        public async Task SeedAsync(ApplicationDbContext context, IWebHostEnvironment env,
            ILogger<ApplicationDbContextSeed> logger, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                if (env.IsDevelopment())
                {
                    if (!context.Users.Any())
                    {
                        await context.Users.AddRangeAsync(GetDefaultUser());

                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(ApplicationDbContext));

                    await SeedAsync(context, env, logger, retryForAvaiability);
                }
            }
        }

        private IEnumerable<ApplicationUser> GetDefaultUser()
        {
            var user =
            new ApplicationUser()
            {
                Email = "user@example.com",
                Id = Guid.NewGuid().ToString(),
                LastName = "User Last Name",
                FirstName = "User First Name",
                PhoneNumber = "1234567890",
                UserName = "user@example.com",
                NormalizedEmail = "USER@EXAMPLE.COM",
                NormalizedUserName = "USER@EXAMPLE.COM",
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");

            return new List<ApplicationUser>()
            {
                user
            };
        }

    }
}