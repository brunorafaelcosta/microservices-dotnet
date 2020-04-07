using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Net;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AccountModels = Services.Identity.STS.Models.Account;

namespace Services.Identity.STS.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly Application.Services.ILoginService _loginService;

        public AccountController(ILogger<AccountController> logger, IConfiguration config,
            IIdentityServerInteractionService interaction,
            Application.Services.ILoginService loginService)
        {
            _logger = logger;
            _configuration = config;
            _interaction = interaction;
            _loginService = loginService;
        }

        #region Login

        /// <summary>
        /// Shows the login page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated == true) {
                return RedirectToAction(nameof(LoggedIn));
            }

            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                throw new NotImplementedException("External login is not implemented!");
            }

            var vm = await BuildLoginViewModelAsync(returnUrl, context);

            ViewData["ReturnUrl"] = returnUrl;

            return View(vm);
        }
        
        /// <summary>
        /// Handles postback from the login page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountModels.LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _loginService.FindByUsername(model.Email);

                if (await _loginService.ValidateCredentials(user, model.Password))
                {
                    var tokenLifetime = 120; //_configuration.GetValue("TokenLifetimeMinutes", 120);

                    var props = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLifetime),
                        AllowRefresh = true,
                        RedirectUri = model.ReturnUrl,
                    };

                    if (model.RememberMe)
                    {
                        var permanentTokenLifetime = 365; //_configuration.GetValue("PermanentTokenLifetimeDays", 365);

                        props.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(permanentTokenLifetime);
                        props.IsPersistent = true;
                    };

                    await _loginService.SignInAsync(user, props);

                    // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
                    if (_interaction.IsValidReturnUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return RedirectToAction(nameof(LoggedIn));
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);

            ViewData["ReturnUrl"] = model.ReturnUrl;

            return View(vm);
        }

        // <summary>
        /// Shows the logged user information page
        /// </summary>
        [HttpGet]
        [Authorize]
        public IActionResult LoggedIn()
        {
            var vm = new AccountModels.LoggedInViewModel
            {
                Name = User.Identity.Name,
                Claims = User.Claims.ToDictionary(claim => claim.Type, claim => claim.Value),
            };

            return View(vm);
        }

        private async Task<AccountModels.LoginViewModel> BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest context)
        {
            // var allowLocal = true;
            // if (context?.ClientId != null)
            // {
            //     var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
            //     if (client != null)
            //     {
            //         allowLocal = client.EnableLocalLogin;
            //     }
            // }

            return new AccountModels.LoginViewModel
            {
                ReturnUrl = returnUrl,
                Email = context?.LoginHint,
            };
        }

        private async Task<AccountModels.LoginViewModel> BuildLoginViewModelAsync(AccountModels.LoginViewModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl, context);
            vm.Email = model.Email;
            vm.RememberMe = model.RememberMe;
            return vm;
        }

        #endregion Login

        #region Logout

        /// <summary>
        /// Shows the logout page
        /// </summary>
        /// <remarks>
        /// this prevents attacks where the user
        /// is automatically signed out by another malicious web page
        /// </remarks>
        [HttpGet]
        [Authorize]
        public IActionResult Logout(string logoutId = null)
        {
            var vm = new AccountModels.LogoutViewModel
            {
                LogoutId = logoutId,
                Name = User.Identity.Name,
            };
            return View(vm);
        }

        /// <summary>
        /// Handles postback from the logout page
        /// </summary>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(AccountModels.LogoutViewModel model)
        {
            var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

            if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                if (model.LogoutId == null)
                {
                    // if there's no current logout context, we need to create one
                    // this captures necessary info from the current logged in user
                    // before we signout and redirect away to the external IdP for signout
                    model.LogoutId = await _interaction.CreateLogoutContextAsync();
                }

                string url = "/Account/Logout?logoutId=" + model.LogoutId;

                try
                {

                    // hack: try/catch to handle social providers that throw
                    await HttpContext.SignOutAsync(idp, new AuthenticationProperties
                    {
                        RedirectUri = url,
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "LOGOUT ERROR: {ExceptionMessage}", ex.Message);
                }
            }

            // delete authentication cookie
            await HttpContext.SignOutAsync();

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);

            if (logout is null || string.IsNullOrEmpty(logout.PostLogoutRedirectUri)) {
                return RedirectToAction(nameof(Login));
            } else {
                return Redirect(logout.PostLogoutRedirectUri);
            }
        }

        #endregion Logout
    }
}
