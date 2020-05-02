using Services.Identity.STS.Core.Domain.Users;
using Transversal.Data.EFCore.Repositories;
using Transversal.Data.EFCore.DbContext;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System;

namespace Services.Identity.STS.Core.Data.Repositories
{
    public class UserRepository : EFCoreRepositoryBase<DefaultDbContext, User, int>,
        IUserRepository
    {
        public UserRepository(IDbContextProvider<DefaultDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            int id = Convert.ToInt32(userId);

            var user = await FirstOrDefaultAsync(id, null);
            
            return user;
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Task.FromResult(FirstOrDefault(e => e.UserName.Equals(normalizedUserName), null));
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user?.UserName);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user?.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user?.UserName);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user?.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user != null && !string.IsNullOrEmpty(user.PasswordHash));
        }

        public void Dispose()
        {
            //throw new System.NotImplementedException();
        }
    }
}
