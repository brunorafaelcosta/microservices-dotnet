using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Resources.API.Models;

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
        [ProducesResponseType(typeof(IEnumerable<ResourceModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok("Used to get resources");
        }

        [HttpGet("{key}")]
        [Authorize]
        [ProducesResponseType(typeof(ResourceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetByKeyAsync(
            string key,
            [FromQuery] string languageCode = null)
        {
            return Ok("Used to get a resource");
        }

        [HttpGet("public")]
        [ProducesResponseType(typeof(IEnumerable<ResourceModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPublicAsync()
        {
            return Ok("Used to get public resources");
        }

        [HttpGet("public/{key}")]
        [ProducesResponseType(typeof(ResourceModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPublicByKeyAsync(
            string key,
            [FromQuery] string languageCode = null)
        {
            return Ok("Used to get a public resource");
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
