using Microsoft.AspNetCore.Identity;
using Transversal.Domain.Repositories;

namespace Services.Identity.STS.Core.Domain.Roles
{
    public interface IRoleRepository : IRepository<Role, int>, IRoleStore<Role>
    {
    }
}
