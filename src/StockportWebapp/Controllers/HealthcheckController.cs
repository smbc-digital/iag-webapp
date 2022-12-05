using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            return Ok(JsonConvert.SerializeObject(healthcheck, new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            }));
        }
    }
}
