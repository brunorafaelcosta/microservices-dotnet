using Services.Identity.STS.Core.Domain.Users;
using System.Linq;
using System.Threading.Tasks;
using Transversal.Data.EFCore.Repositories;
using Transversal.Data.EFCore.DbContext;

namespace Services.Identity.STS.Core.Data.Repositories
{
    public class UserPictureRepository : EFCoreRepositoryBase<DefaultDbContext, UserPicture, int>,
        IUserPictureRepository
    {
        public UserPictureRepository(IDbContextProvider<DefaultDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public UserPicture GetByUserId(int userId)
        {
            return AsQueryable().Single(e => e.UserId == userId);
        }

        public Task<UserPicture> GetByUserIdAsync(int userId)
        {
            return Task.FromResult(GetByUserId(userId));
        }
    }
}
