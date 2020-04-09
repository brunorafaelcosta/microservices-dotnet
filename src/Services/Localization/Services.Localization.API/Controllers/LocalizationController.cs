using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Services.Localization.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LocalizationController : ControllerBase
    {
        private readonly ILogger<LocalizationController> _logger;

        public LocalizationController(ILogger<LocalizationController> logger)
        {
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
