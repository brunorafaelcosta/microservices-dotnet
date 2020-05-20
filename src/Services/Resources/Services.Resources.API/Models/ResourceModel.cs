using Services.Resources.API.Core.Application.Dto.Resources;
using Transversal.Web.API.Models;
using Transversal.Web.API.Models.Response;

namespace Services.Resources.API.Models
{
    public class ResourceModel : 
        IModelFromAppDto<ResourceDto>,
        IResponseModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public ResourceModel()
            : this(null)
        {
        }

        public ResourceModel(ResourceDto dto)
        {
            FromAppDto(dto);
        }

        public void FromAppDto(ResourceDto dto)
        {
            if (dto is null)
                return;

            // TODO: use automapper here!
            Key = dto.Key;
            Value = dto.Value;
            Description = dto.Description;
        }
    }
}
