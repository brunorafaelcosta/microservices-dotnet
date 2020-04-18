namespace Services.Localization.API.Core.Application.Dto
{
    public class ResourceGroupDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        internal static ResourceGroupDto FromEntity(Domain.Resources.ResourceGroup entity)
        {
            return new ResourceGroupDto
            {
                Name = entity.Name,
                Description = entity.Description
            };
        }
    }
}
