using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Identity.STS.Core.Domain.Users;
using System.Threading.Tasks;
using Transversal.Domain.Uow.Manager;

namespace Services.Identity.STS.Core.Application.CustomIdentity
{
    public class CustomSecurityStampValidator : SecurityStampValidator<User>
    {
        protected IUnitOfWorkManager UowManager { get; private set; }

        public CustomSecurityStampValidator(
            IUnitOfWorkManager uowManager,
            IOptions<SecurityStampValidatorOptions> options,
            SignInManager<User> signInManager,
            ISystemClock systemClock,
            ILoggerFactory loggerFactory)
            : base(
                options,
                signInManager,
                systemClock,
                loggerFactory)
        {
            UowManager = uowManager;
        }

        public override Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            using (var uow = UowManager.Begin())
            {
                return base.ValidateAsync(context);
            }
        }
    }
}
