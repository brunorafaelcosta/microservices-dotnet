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

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                throw new NotImplementedException("External login is not implemented!");
            }

            var vm = await BuildLoginViewModelAsync(returnUrl, context);

            ViewData["ReturnUrl"] = returnUrl;

            return View(vm);
        }
        
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
                        RedirectUri = model.ReturnUrl
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

                    return RedirectToAction("LoggedIn");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);

            ViewData["ReturnUrl"] = model.ReturnUrl;

            return View(vm);
        }

        private async Task<AccountModels.LoginViewModel> BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest context)
        {
            var allowLocal = true;
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


        [HttpGet]
        [Authorize]
        public IActionResult LoggedIn()
        {
            // _logger.LogInformation("Http request access to {Resource}", $"{nameof(AccountController)}.{nameof(Get)}");

            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
