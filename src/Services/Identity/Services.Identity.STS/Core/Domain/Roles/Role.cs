using Transversal.Domain.Entities;

namespace Services.Identity.STS.Core.Domain.Roles
{
    public class Role : AuditedAggregateRoot<int>
    {
        public string Name { get; set; }
    }
}
