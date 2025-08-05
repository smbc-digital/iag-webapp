namespace StockportWebapp.Controllers;

[Route("[controller]")]
public class ShedController(ShedApiClient client) : Controller
{
    private readonly ShedApiClient _client = client;

    [HttpGet("data")]
    public async Task<IActionResult> GetData(string ward, string listingType)
    {
        var result = await _client.GetSHEDData(ward, listingType);
        return Ok(result);
    }

    [HttpGet("by-id")]
    public async Task<IActionResult> GetShedDataById([FromQuery] string id)
    {
        var result = await _client.GetSHEDDataByID(id);
        return Ok(result);
    }

    [HttpGet("by-name")]
    public async Task<IActionResult> GetShedDataByName([FromQuery] string id)
    {
        var result = await _client.GetSHEDDataByName(id);
        return Ok(result);
    }
}