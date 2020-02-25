using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Services.Identity.STS.Application.Models;

namespace Services.Identity.STS.Application.Services
{
    public interface ILoginService
    {
        Task<bool> ValidateCredentials(ApplicationUser user, string password);

        Task<ApplicationUser> FindByUsername(string user);

        Task SignIn(ApplicationUser user);

        Task SignInAsync(ApplicationUser user, AuthenticationProperties properties, string authenticationMethod = null);
    }
}