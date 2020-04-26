using Transversal.Domain.Entities;
using Transversal.Domain.Entities.Tenancy;

namespace Services.Identity.STS.Core.Domain.Users
{
    public class User : AuditedAggregateRoot<int>,
        IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public bool HasTenant => TenantId.HasValue;

        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        public UserStatus Status { get; set; }

        public int AccessFailedCount { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }

        public UserPicture Picture { get; set; }
        public bool HasPicture { get; set; }

        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public string Roles { get; set; }
        public string Permissions { get; set; }

        public bool IsActive()
        {
            return this.Status == UserStatus.Active;
        }
    }
}
