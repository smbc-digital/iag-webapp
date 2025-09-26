
namespace StockportWebapp.Services;

public interface IShedService
{
    Task<List<ShedItem>> GetSHEDDataByHeRef(string name);
    Task<List<ShedItem>> GetSHEDDataByNameWardsAndListingTypes(string name, List<string> ward, List<string> listingTypes);
}

public class ShedService(IShedApiClient shedApiClient, MarkdownWrapper markdownWrapper) : IShedService
{
    private readonly IShedApiClient _shedApiClient = shedApiClient;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

    public async Task<List<ShedItem>> GetSHEDDataByHeRef(string name)
    {
        string json = await _shedApiClient.GetSHEDDataByHeRef(name);

        List<ShedItem> assets = System.Text.Json.JsonSerializer.Deserialize<List<ShedItem>>(json);

        ShedItem shedItem = assets.FirstOrDefault();

        if (shedItem is not null)
            shedItem.Description = _markdownWrapper.ConvertToHtml(shedItem.Description ?? string.Empty);

        return assets ?? new List<ShedItem>();
    }

    public async Task<List<ShedItem>> GetSHEDDataByNameWardsAndListingTypes(string name, List<string> ward, List<string> listingTypes)
    {
        string json = await _shedApiClient.GetSHEDDataByNameWardsAndListingTypes(name, ward, listingTypes);

        List<ShedItem> assets = System.Text.Json.JsonSerializer.Deserialize<List<ShedItem>>(json);

        return assets ?? new List<ShedItem>();
    }
}