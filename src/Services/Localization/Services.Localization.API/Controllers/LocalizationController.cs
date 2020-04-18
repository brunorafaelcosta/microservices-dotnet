using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Services.Localization.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LocalizationController : ControllerBase
    {
        private readonly ILogger<LocalizationController> _logger;
        private readonly Core.Application.IResourcesApplicationService _service;

        public LocalizationController(
            Core.Application.IResourcesApplicationService service,
            ILogger<LocalizationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Test Action
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Index()
        {
            return Ok("It works!");
        }
    }
}
