using System.Collections.Generic;
using Transversal.Domain.Entities;
using Transversal.Domain.Entities.Tenancy;
using Transversal.Domain.ValueObjects.Localization;

namespace Services.Resources.API.Core.Domain
{
    public class Resource : AuditedEntity,
        IMayHaveTenant<Resource, int>
    {
        public int? TenantId { get; set; }
        public Resource TenantlessEntity { get; set; }
        public ICollection<Resource> TenantEntities { get; set; }

        public string Key { get; set; }
        public LocalizedValueObject Value { get; set; }
        public string Description { get; set; }

        public int ResourceGroupId { get; set; }
        public virtual ResourceGroup ResourceGroup { get; set; }
    }
}
