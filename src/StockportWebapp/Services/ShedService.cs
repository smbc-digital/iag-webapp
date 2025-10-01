namespace StockportWebapp.Services;

public interface IShedService
{
    Task<ShedItem> GetSHEDDataByHeRef(string heRef);
    Task<List<ShedItem>> GetSHEDDataByNameWardsAndListingTypes(string heRef, List<string> ward, List<string> listingTypes);
}

public class ShedService(IShedApiClient shedApiClient, MarkdownWrapper markdownWrapper) : IShedService
{
    private readonly IShedApiClient _shedApiClient = shedApiClient;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

    public async Task<ShedItem> GetSHEDDataByHeRef(string heRef)
    {
        string json = await _shedApiClient.GetSHEDDataByHeRef(heRef);

        if (string.IsNullOrEmpty(json))
            return null;

        ShedItem shedItem = System.Text.Json.JsonSerializer.Deserialize<ShedItem>(json);

        if (shedItem is not null)
            shedItem.Description = _markdownWrapper.ConvertToHtml(shedItem.Description ?? string.Empty);

        return shedItem ?? null;
    }

    public async Task<List<ShedItem>> GetSHEDDataByNameWardsAndListingTypes(string name, List<string> ward, List<string> listingTypes)
    {
        string json = await _shedApiClient.GetSHEDDataByNameWardsAndListingTypes(name, ward, listingTypes);

        List<ShedItem> assets = System.Text.Json.JsonSerializer.Deserialize<List<ShedItem>>(json);

        return assets ?? new List<ShedItem>();
    }
}