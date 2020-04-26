using Microsoft.AspNetCore.Authentication;
using Services.Identity.STS.Core.Domain.Users;
using System.Threading.Tasks;
using Transversal.Application;

namespace Services.Identity.STS.Core.Application
{
    public interface IUsersApplicationService : IApplicationService
    {
        Task<bool> ValidateCredentialsAsync(User user, string password);

        Task<User> FindByUsernameAsync(string username);

        Task SignInAsync(User user, AuthenticationProperties properties, string authenticationMethod = null);

        Task<Dto.UserPictureDto> GetUserPictureAsync(int userId);
    }
}
