using Microsoft.Extensions.Configuration;

namespace Transversal.Data.EFCore.DbContext
{
    public class DefaultConnectionStringResolver : IConnectionStringResolver
    {
        public const string DefaultConfigurationConnectionStringName = "Sql";

        private readonly IConfiguration _configuration;

        public DefaultConnectionStringResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetNameOrConnectionString<TDbContext>()
        {
            return _configuration.GetConnectionString(DefaultConfigurationConnectionStringName);
        }
    }
}
