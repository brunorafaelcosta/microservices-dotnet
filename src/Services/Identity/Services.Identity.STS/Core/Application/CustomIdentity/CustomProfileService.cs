using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Services.Identity.STS.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Transversal.Application.Exceptions;
using Transversal.Common.Extensions;
using Transversal.Domain.Uow.Manager;
using Transversal.Web.Session.Identity;

namespace Services.Identity.STS.Core.Application.CustomIdentity
{
    public class CustomProfileService : IProfileService
    {
        private const string PictureEndpoint = "/Account/GetPicture";

        private readonly IUnitOfWorkManager _uowManager;
        private readonly Settings _settings;
        private readonly UserManager<User> _userManager;
        
        public CustomProfileService(
            IUnitOfWorkManager uowManager,
            Settings settings,
            UserManager<User> userManager)
        {
            _uowManager = uowManager ?? throw new ArgumentNullException(nameof(uowManager));
            _settings = settings ?? throw new ArgumentException(nameof(settings));
            _userManager = userManager ?? throw new ArgumentException(nameof(userManager));
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            using (var uow = _uowManager.Begin())
            {
                var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
                var userId = subject.Claims.Where(x => x.Type == JwtClaimTypes.Subject).FirstOrDefault().Value;

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new AppException("Invalid subject identifier");

                var claims = GetClaimsFromUser(user);
                context.IssuedClaims = claims?.ToList() ?? new List<Claim>();
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            using (var uow = _uowManager.Begin())
            {
                context.IsActive = false;

                var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
                var userId = subject.Claims.Where(x => x.Type == JwtClaimTypes.Subject).FirstOrDefault().Value;

                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    //if (_userManager.SupportsUserSecurityStamp)
                    //{
                    //    var security_stamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                    //    if (security_stamp != null)
                    //    {
                    //        var db_security_stamp = await _userManager.GetSecurityStampAsync(user);
                    //        if (db_security_stamp != security_stamp)
                    //            return;
                    //    }
                    //}

                    context.IsActive = user.IsActive();
                }
            }
        }

        private IEnumerable<Claim> GetClaimsFromUser(User user)
        {
            List<Claim> claims = null;

            if (user != null)
            {
                claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                    new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                };

                var userId = user.Id.ToString();
                claims.Add(new Claim(ClaimsConstants.UserIdClaimType, userId));

                var tenantId = user.HasTenant ? user.TenantId.Value.ToString() : ClaimsConstants.NullValue;
                claims.Add(new Claim(ClaimsConstants.TenantIdClaimType, tenantId));

                var name = !string.IsNullOrWhiteSpace(user.Name) ? user.Name : ClaimsConstants.NullValue;
                claims.Add(new Claim(ClaimsConstants.NameClaimType, name));

                var lastName = !string.IsNullOrWhiteSpace(user.LastName) ? user.LastName : ClaimsConstants.NullValue;
                claims.Add(new Claim(ClaimsConstants.NameClaimType, lastName));

                var userPictureUrl = user.HasPicture
                    ? UrlExtensions.Combine(_settings.ExternalEndpoint, PictureEndpoint)
                    : ClaimsConstants.NullValue;
                claims.Add(new Claim(JwtClaimTypes.Picture, userPictureUrl));

                if (_userManager.SupportsUserEmail)
                {
                    string email = !string.IsNullOrWhiteSpace(user.Email) ? user.Email : ClaimsConstants.NullValue;
                    string emailConfirmed = user.EmailConfirmed ? ClaimsConstants.TrueValue : ClaimsConstants.FalseValue;

                    claims.AddRange(new[]
                    {
                        new Claim(JwtClaimTypes.Email, email),
                        new Claim(JwtClaimTypes.EmailVerified, emailConfirmed, ClaimValueTypes.Boolean)
                    });
                }

                if (_userManager.SupportsUserPhoneNumber)
                {
                    string phoneNumber = !string.IsNullOrWhiteSpace(user.PhoneNumber) ? user.PhoneNumber : ClaimsConstants.NullValue;
                    string phoneNumberConfirmed = user.PhoneNumberConfirmed ? ClaimsConstants.TrueValue : ClaimsConstants.FalseValue;

                    claims.AddRange(new[]
                    {
                        new Claim(JwtClaimTypes.PhoneNumber, phoneNumber),
                        new Claim(JwtClaimTypes.PhoneNumberVerified, phoneNumberConfirmed, ClaimValueTypes.Boolean)
                    });
                }

                var roles = !string.IsNullOrWhiteSpace(user.Roles) ? user.Roles : ClaimsConstants.NullValue;
                claims.Add(new Claim(ClaimsConstants.RolesClaimType, roles));

                var permissions = !string.IsNullOrWhiteSpace(user.Permissions) ? user.Permissions : ClaimsConstants.NullValue;
                claims.Add(new Claim(ClaimsConstants.PermissionsClaimType, permissions));
            }

            return claims;
        }
    }
}
