﻿using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Transversal.Web.Grpc.Client;
using AccountModels = Services.Identity.STS.Models.Account;

namespace Services.Identity.STS.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly Settings _settings;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly Core.Application.IUsersApplicationService _usersApplicationService;

        public AccountController(IWebHostEnvironment env, ILogger<AccountController> logger, IConfiguration config, Settings settings,
            IIdentityServerInteractionService interaction,
            Core.Application.IUsersApplicationService usersApplicationService)
        {
            _env = env;
            _logger = logger;
            _configuration = config;
            _settings = settings;
            _interaction = interaction;
            _usersApplicationService = usersApplicationService;
        }

        #region Login

        /// <summary>
        /// Shows the login page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var loginKey = await GrpcCallerService.CallService(_settings.InternalEndpoints["grpcResources"], _logger, async channel =>
            {
                var client = new Resources.API.Grpc.ResourceService.ResourceServiceClient(channel);
                var request = new Resources.API.Grpc.GetByKeyRequest() { Key = "login_lb", LanguageCode = "fr" };
                _logger.LogInformation("grpc client created, request = {@request}", request);
                var reply = await client.GetByKeyAsync(request);
                _logger.LogInformation("grpc reply {@reply}", reply);
                return reply?.Value;
            });

            if (User.Identity.IsAuthenticated == true) {
                return RedirectToAction(nameof(LoggedIn));
            }

            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                throw new NotImplementedException("External login is not implemented!");
            }

            var vm = BuildLoginViewModel(returnUrl, context);

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
                var user = await _usersApplicationService.FindByUsernameAsync(model.Email);

                if (await _usersApplicationService.ValidateCredentialsAsync(user, model.Password))
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

                    await _usersApplicationService.SignInAsync(user, props);

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

        private AccountModels.LoginViewModel BuildLoginViewModel(string returnUrl, AuthorizationRequest context)
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
            var vm = BuildLoginViewModel(model.ReturnUrl, context);
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

        /// <summary>
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

        /// <summary>
        /// Gets the logged user profilepicture
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ProfilePictureAsync()
        {
            var subjectId = User.Identity.GetSubjectId();
            var userId = Convert.ToInt32(subjectId);

            var userPictureDto = await _usersApplicationService.GetUserPictureAsync(userId);
            
            if (userPictureDto != null)
            {
                return File(userPictureDto.Data, userPictureDto.MimeType);
            }

            return NotFound();
        }
    }
}
