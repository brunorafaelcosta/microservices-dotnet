using Transversal.Domain.Entities;
using Transversal.Domain.Entities.Tenancy;
using Transversal.Domain.ValueObjects.Localization;

namespace Services.Localization.API.Core.Domain.Resources
{
    public class Resource : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public string Key { get; set; }
        public LocalizedValueObject Value { get; set; }
        public string Description { get; set; }

        public int ResourceGroupId { get; set; }
        public virtual ResourceGroup ResourceGroup { get; set; }
    }
}
