using Services.Localization.API.Core.Application.Dto.Resources;
using Services.Localization.API.Core.Domain.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Transversal.Application.Dto;
using Transversal.Common.Projection;

namespace Services.Localization.API.Core.Application.Dto.ResourceGroups
{
    public class ResourceGroupDto : Projectable<ResourceGroup, ResourceGroupDto>,
        IDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ResourceDto> Resources { get; set; }

        #region Projection

        public static new Expression<Func<ResourceGroup, ResourceGroupDto>> Projection
        {
            get
            {
                return e => new ResourceGroupDto
                {
                    Name = e.Name,
                    Description = e.Description,
                    Resources = e.Resources.AsQueryable().Select(e => ResourceDto.FromEntity(e)).ToList()
                };
            }
        }

        #endregion Projection
    }
}
