using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Services.Identity.STS.Application.Models;

namespace Services.Identity.STS.Application.Services
{
    public class LoginService : ILoginService
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        
        public LoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> ValidateCredentials(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
        
        public async Task<ApplicationUser> FindByUsername(string user)
        {
            return await _userManager.FindByEmailAsync(user);
        }

        public Task SignIn(ApplicationUser user)
        {
            return _signInManager.SignInAsync(user, true);
        }

        public Task SignInAsync(ApplicationUser user, AuthenticationProperties properties, string authenticationMethod = null)
        {
            return _signInManager.SignInAsync(user, properties, authenticationMethod);
        }
    }
}