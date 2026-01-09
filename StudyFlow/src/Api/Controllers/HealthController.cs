using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyFlow.src.Application.Abstractions;

namespace StudyFlow.src.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IHealthService _healthService;

        public HealthController(IHealthService healthService)
        {
            _healthService = healthService;
        }
        // GET: api/Health
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_healthService.GetHealth());
        }
    }
}
