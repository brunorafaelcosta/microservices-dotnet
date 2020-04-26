using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Services.Identity.STS.Core.Domain.Users;
using System;
using System.Threading.Tasks;
using Transversal.Application;
using Transversal.Common.Extensions;
using Transversal.Common.Session;
using Transversal.Domain.Uow.Manager;

namespace Services.Identity.STS.Core.Application
{
    public class UsersApplicationService : ApplicationServiceBase, IUsersApplicationService
    {
        private readonly IUserPictureRepository _userPictureRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        
        public UsersApplicationService(
            ISessionInfo session,
            IUnitOfWorkManager uowManager,
            IUserPictureRepository userPictureRepository,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
            : base(session, uowManager)
        {
            _userPictureRepository = userPictureRepository ?? throw new ArgumentNullException(nameof(userPictureRepository));
            _userManager = userManager ?? throw new ArgumentException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentException(nameof(signInManager));
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
                        MimeType = FileExtensions.GetImageMimeTypeFromImageFileExtension(userPicture.Extension),
                        Data = userPicture.Data
                    };
                }

                return result;
            }
        }
    }
}
