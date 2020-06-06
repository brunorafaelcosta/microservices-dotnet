using GrpcServiceBase = Services.Resources.API.Grpc.ResourceService.ResourceServiceBase;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Services.Resources.API.Core.Application.Dto.Resources;
using System;
using System.Threading.Tasks;

namespace Services.Resources.API.Grpc.Implementations
{
    public class ResourceService : GrpcServiceBase
    {
        private readonly ILogger<ResourceService> _logger;
        private readonly Core.Application.IResourcesAppService _resourcesAppService;

        public ResourceService(
            ILogger<ResourceService> logger,
            Core.Application.IResourcesAppService resourcesAppService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _resourcesAppService = resourcesAppService ?? throw new ArgumentNullException(nameof(resourcesAppService));
        }

        public override async Task<GetByKeyReply> GetByKey(GetByKeyRequest request, ServerCallContext context)
        {
            try
            {
                var appRequest = new ResourceRequestDto
                {
                    WithKey = request.Key,
                    LanguageCode = request.LanguageCode
                };
                var appResponse = await _resourcesAppService.GetAsync(appRequest);

                var response = new GetByKeyReply
                {
                    Value = appResponse.Value
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return null;
            }
        }

        public override async Task<GetByKeyReply> GetPublicByKey(GetByKeyRequest request, ServerCallContext context)
        {
            try
            {
                var appRequest = new ResourceRequestDto
                {
                    WithKey = request.Key,
                    LanguageCode = request.LanguageCode,
                    MustBePublic = true
                };
                var appResponse = await _resourcesAppService.GetAsync(appRequest);

                var response = new GetByKeyReply
                {
                    Value = appResponse.Value
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return null;
            }
        }
    }
}
