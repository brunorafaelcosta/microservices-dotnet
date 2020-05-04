using Services.Localization.API.Core.Domain.Resources;
using System;
using System.Linq.Expressions;
using Transversal.Application.Dto;
using Transversal.Common.Projection;

namespace Services.Localization.API.Core.Application.Dto.Resources
{
    public class ResourceDto : Projectable<Resource, ResourceDto>,
        IDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        #region Projection

        public static new Expression<Func<Resource, ResourceDto>> Projection
        {
            get
            {
                return e => new ResourceDto
                {
                    Key = e.Key,
                    Value = e.Value.InvariantValue,
                    Description = e.Description
                };
            }
        }

        #endregion Projection
    }
}
