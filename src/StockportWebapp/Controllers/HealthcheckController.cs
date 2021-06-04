using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockportWebapp.Services;

namespace StockportWebapp.Controllers
{
    public class HealthcheckController : Controller
    {
        private readonly IHealthcheckService _healthCheckService;

        public HealthcheckController(IHealthcheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [ResponseCacheAttribute(NoStore = true)]
        [Route("/_healthcheck")]
        public async Task<IActionResult> Index()
        {
            var healthcheck = await _healthCheckService.Get();
            return Ok(JsonConvert.SerializeObject(healthcheck));
        }
    }
}
