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
using Transversal.Application.Exceptions;
using Transversal.Common.Session;
using Transversal.Domain.Uow.Manager;
using Transversal.Web.Session.Identity;

namespace Services.Identity.STS.Core.Application
{
    public class UsersApplicationService : ApplicationServiceBase, IUsersApplicationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly IUserPictureRepository _userPictureRepository;

        public UsersApplicationService(
            ISessionInfo session,
            IUnitOfWorkManager uowManager,
            IUserPictureRepository userPictureRepository,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
            : base(session, uowManager)
        {
            _userPictureRepository = userPictureRepository ?? throw new ArgumentNullException(nameof(userPictureRepository));
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

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            using (var uow = UowManager.Begin())
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

        public async Task<Dto.UserPictureDto> GetUserPictureAsync(int userId)
        {
            using (var uow = UowManager.Begin())
            {
                Dto.UserPictureDto result = null;

                var userPicture = await _userPictureRepository.GetByUserIdAsync(userId);
                if (userPicture != null)
                {
                    result = new Dto.UserPictureDto
                    {
                        MimeType = Transversal.Common.Extensions.FileExtensions.GetImageMimeTypeFromImageFileExtension(userPicture.Extension),
                        Data = userPicture.Data
                    };
                }

                return result;
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

                var userPictureUrl = user.HasPicture ? "http:localhost:5101/Account/GetPictureAsync" : ClaimsConstants.NullValue;
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
