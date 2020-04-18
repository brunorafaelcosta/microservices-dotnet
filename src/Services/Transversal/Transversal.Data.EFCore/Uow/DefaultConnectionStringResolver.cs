using Microsoft.Extensions.Configuration;
using Transversal.Domain.Uow;

namespace Transversal.Data.EFCore.Uow
{
    public class DefaultConnectionStringResolver : IConnectionStringResolver
    {
        private readonly IConfiguration _configuration;

        public DefaultConnectionStringResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetNameOrConnectionString()
        {
            return _configuration.GetConnectionString("Sql");
        }
    }
}
