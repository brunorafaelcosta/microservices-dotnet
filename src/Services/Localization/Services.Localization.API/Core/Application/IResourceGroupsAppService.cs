﻿using Services.Localization.API.Core.Application.Dto;
using Transversal.Application;
using Transversal.Application.Request;
using Transversal.Application.Response;

namespace Services.Localization.API.Core.Application
{
    public interface IResourceGroupsAppService : IApplicationService
    {
        IResponse<ResourceGroupDto> Get(int id);

        IPaginatedResponse<ResourceGroupDto> GetAll(ILocalizedPaginatedRequest<RequestResourceGroupDto> request);

        IResponse<int?> Create(CreateResourceGroupDto input);
        
        IResponse<int> Update(UpdateResourceGroupDto input);
        
        IResponse<bool> Delete(int id);
    }
}
