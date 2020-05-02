using Services.Localization.API.Core.Domain.Resources;
using System;
using System.Linq.Expressions;
using Transversal.Application.Dto;

namespace Services.Localization.API.Core.Application.Dto
{
    public class ResourceGroupDto : IDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        internal static Expression<Func<ResourceGroup, ResourceGroupDto>> Projection
        {
            get
            {
                return e => new ResourceGroupDto
                {
                    Name = e.Name,
                    Description = e.Description
                };
            }
        }
    }
}
