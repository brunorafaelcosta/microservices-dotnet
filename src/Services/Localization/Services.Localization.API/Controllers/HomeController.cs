using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Services.Localization.API.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
