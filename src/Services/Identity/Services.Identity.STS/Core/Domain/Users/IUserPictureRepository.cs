using System.Threading.Tasks;
using Transversal.Domain.Repositories;

namespace Services.Identity.STS.Core.Domain.Users
{
    public interface IUserPictureRepository : IRepository<UserPicture, int>
    {
        UserPicture GetByUserId(int userId);
        Task<UserPicture> GetByUserIdAsync(int userId);
    }
}
