using Microsoft.AspNetCore.Identity;
using Transversal.Domain.Repositories;

namespace Services.Identity.STS.Core.Domain.Users
{
    public interface IUserRepository : IRepository<User, int>,
        IUserStore<User>, IUserPasswordStore<User>
    {
    }
}
