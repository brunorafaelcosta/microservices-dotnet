using Transversal.Domain.Entities;

namespace Services.Identity.STS.Core.Domain.Users
{
    public class User : AuditedAggregateRoot<int>
    {
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }
    }
}
