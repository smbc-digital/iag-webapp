namespace StockportWebapp.Controllers;

public class HealthcheckController(IHealthcheckService healthCheckService) : Controller
{
    private readonly IHealthcheckService _healthCheckService = healthCheckService;

    [ResponseCache(NoStore = true)]
    [Route("/_healthcheck")]
    public async Task<IActionResult> Index()
    {
        Healthcheck healthcheck = await _healthCheckService.Get();
        DefaultContractResolver contractResolver = new()
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        return Ok(JsonConvert.SerializeObject(healthcheck, new JsonSerializerSettings
        {
            ContractResolver = contractResolver
        }));
    }
}