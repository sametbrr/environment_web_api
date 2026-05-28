using Microsoft.AspNetCore.Mvc;

namespace EnvironmentWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<TestController> _logger;

        public TestController(IHostEnvironment env, ILogger<TestController> logger)
        {
            _env = env;
            _logger = logger;
        }

        [HttpGet("environment-name")]
        public ActionResult<string> GetEnvironmentName()
        {
            return Ok(_env.EnvironmentName);
        }
    }
}
