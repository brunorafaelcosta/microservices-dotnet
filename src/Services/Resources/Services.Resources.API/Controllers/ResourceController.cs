using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Resources.API.Core.Application.Dto.Resources;
using Services.Resources.API.Models;
using Transversal.Web.API.Models.Response;

namespace Services.Resources.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly ILogger<ResourceController> _logger;
        private readonly Core.Application.IResourcesAppService _resourcesAppService;

        public ResourceController(
            ILogger<ResourceController> logger,
            Core.Application.IResourcesAppService resourcesAppService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _resourcesAppService = resourcesAppService ?? throw new ArgumentNullException(nameof(resourcesAppService));
        }

        #region GET

        [HttpGet()]
        [Authorize]
        [ProducesResponseType(typeof(IPaginatedResponseModel<ResourceModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetAsync([FromQuery] ResourceRequestModel request)
        {
            try
            {
                // TODO: Use automapper here!
                var appRequest = new ResourcePaginatedRequestDto
                {
                    WithId = request.WithId,
                    WithKey = request.WithKey
                    //LanguageCode = request.lang
                };
                var appResponse = await _resourcesAppService.GetAllAsync(appRequest);
                
                var response = appResponse.ToPaginatedResponseModel<ResourceModel, ResourceDto>();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{key}")]
        [Authorize]
        [ProducesResponseType(typeof(ResourceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetByKeyAsync(
            string key,
            [FromQuery] string languageCode = null)
        {
            try
            {
                var appRequest = new ResourceRequestDto
                {
                    WithKey = key,
                    LanguageCode = languageCode
                };
                var appResponse = await _resourcesAppService.GetAsync(appRequest);

                var response = appResponse.ToResponseModel<ResourceModel, ResourceDto>();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("public")]
        [ProducesResponseType(typeof(IPaginatedResponseModel<ResourceModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetPublicAsync([FromQuery] ResourceRequestModel request)
        {
            try
            {
                // TODO: Use automapper here!
                var appRequest = new ResourcePaginatedRequestDto
                {
                    WithId = request.WithId,
                    WithKey = request.WithKey,
                    MustBePublic = true
                    //LanguageCode = request.lang
                };
                var appResponse = await _resourcesAppService.GetAllAsync(appRequest);

                var response = appResponse.ToPaginatedResponseModel<ResourceModel, ResourceDto>();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("public/{key}")]
        [ProducesResponseType(typeof(ResourceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetPublicByKeyAsync(
            string key,
            [FromQuery] string languageCode = null)
        {
            try
            {
                var appRequest = new ResourceRequestDto
                {
                    WithKey = key,
                    LanguageCode = languageCode,
                    MustBePublic = true
                };
                var appResponse = await _resourcesAppService.GetAsync(appRequest);

                var response = appResponse.ToResponseModel<ResourceModel, ResourceDto>();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion GET

        #region POST

        [HttpPost("{key}")]
        [Authorize]
        [ProducesResponseType(typeof(ResourceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAsync(string key)
        {
            return Ok("Used to modify and update a resource");
        }

        #endregion POST

        #region PUT

        [HttpPut]
        [Authorize]
        [ProducesResponseType(typeof(ResourceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutAsync()
        {
            return Ok("Used to create a resource");
        }

        #endregion PUT

        #region DELETE

        [HttpDelete("{key}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync(string key)
        {
            return Ok("Used to delete a resource");
        }

        #endregion DELETE
    }
}
