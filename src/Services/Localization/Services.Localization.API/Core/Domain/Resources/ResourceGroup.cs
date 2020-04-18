using System.Collections.Generic;
using Transversal.Domain.Entities;

namespace Services.Localization.API.Core.Domain.Resources
{
    public class ResourceGroup : AuditedAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }

        public int? ParentResourceGroupId { get; set; }
        public virtual ResourceGroup ParentResourceGroup { get; set; }
        
        public virtual ICollection<Resource> Resources { get; set; }
    }
}
