using IdentityModel;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Services.Identity.STS.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Transversal.Application;
using Transversal.Common.Session;
using Transversal.Domain.Uow.Manager;

namespace Services.Identity.STS.Core.Application
{
    public class UsersApplicationService : ApplicationServiceBase, IUsersApplicationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UsersApplicationService(
            ISessionInfo session,
            IUnitOfWorkManager uowManager,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
            : base(session, uowManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task SignInAsync(User user, AuthenticationProperties properties, string authenticationMethod = null)
        {
            using (var uow = UowManager.Begin())
            {
                return _signInManager.SignInAsync(user, properties, authenticationMethod);
            }
        }

        public async Task<bool> ValidateCredentialsAsync(User user, string password)
        {
            using (var uow = UowManager.Begin())
            {
                return await _userManager.CheckPasswordAsync(user, password);
            }
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            using (var uow = UowManager.Begin())
            {
                return await _userManager.FindByNameAsync(username);
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            using (var uow = UowManager.Begin())
            {
                var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

                var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
                var user = await _userManager.FindByIdAsync(subjectId);

                context.IsActive = false;

                if (user != null)
                {
                    if (_userManager.SupportsUserSecurityStamp)
                    {
                        var security_stamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                        if (security_stamp != null)
                        {
                            var db_security_stamp = await _userManager.GetSecurityStampAsync(user);
                            if (db_security_stamp != security_stamp)
                                return;
                        }
                    }

                    context.IsActive = true;
                    //context.IsActive =
                    //    !user.LockoutEnabled ||
                    //    !user.LockoutEnd.HasValue ||
                    //    user.LockoutEnd <= DateTime.Now;
                }
            }
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            using (var uow = UowManager.Begin())
            {
                var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

                var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;

                var user = await _userManager.FindByIdAsync(subjectId);
                if (user == null)
                    throw new ArgumentException("Invalid subject identifier");

                var claims = GetClaimsFromUser(user);
                context.IssuedClaims = claims.ToList();
            }
        }


        private IEnumerable<Claim> GetClaimsFromUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            claims.Add(new Claim(Transversal.Web.Session.Identity.ClaimsConstants.UserIdClaimType, 1.ToString()));
            claims.Add(new Claim(Transversal.Web.Session.Identity.ClaimsConstants.TenantIdClaimType, string.Empty));

            if (!string.IsNullOrWhiteSpace(user.Name))
                claims.Add(new Claim("name", user.Name));

            if (!string.IsNullOrWhiteSpace(user.LastName))
                claims.Add(new Claim("last_name", user.LastName));

            //if (_userManager.SupportsUserEmail)
            //{
            //    claims.AddRange(new[]
            //    {
            //        new Claim(JwtClaimTypes.Email, user.Email),
            //        new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
            //    });
            //}

            //if (_userManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
            //{
            //    claims.AddRange(new[]
            //    {
            //        new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
            //        new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
            //    });
            //}

            claims.Add(new Claim("permissions", "permission1;permission2"));

            return claims;
        }
    }
}
