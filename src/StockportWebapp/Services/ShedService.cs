namespace StockportWebapp.Services;

using System.Text.Json;
using StockportWebapp.Client;

public class ShedService(ShedApiClient shedApiClient, MarkdownWrapper markdownWrapper)
{
    private readonly ShedApiClient _shedApiClient = shedApiClient;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

    public async Task<List<ShedItem>> GetShedData(string ward, string listingType)
    {
        string json = await _shedApiClient.GetSHEDData(ward, listingType);

        List<ShedItem> assets = JsonSerializer.Deserialize<List<ShedItem>>(json);

        return assets ?? new List<ShedItem>();
    }

    public async Task<List<ShedItem>> GetShedDataById(string id)
    {
        string json = await _shedApiClient.GetSHEDDataByID(id);

        List<ShedItem> assets = JsonSerializer.Deserialize<List<ShedItem>>(json);

        return assets ?? new List<ShedItem>();
    }

    public async Task<List<ShedItem>> GetShedDataByName(string name)
    {
        string json = await _shedApiClient.GetSHEDDataByName(name);

        List<ShedItem> assets = JsonSerializer.Deserialize<List<ShedItem>>(json);

        ShedItem shedItem = assets.FirstOrDefault();
        shedItem.Description = _markdownWrapper.ConvertToHtml(shedItem.Description ?? string.Empty);

        return assets;
    }

    public async Task<List<ShedItem>> GetAllSHEDData()
    {
        string json = await _shedApiClient.GetAllSHEDData();

        List<ShedItem> assets = JsonSerializer.Deserialize<List<ShedItem>>(json);

        return assets ?? new List<ShedItem>();
    }

    public async Task<List<ShedItem>> GetSHEDDataByWardsAndListingTypes(List<string> ward, List<string> listingTypes)
    {
        string json = await _shedApiClient.GetSHEDDataByWardsAndListingTypes(ward, listingTypes);

        List<ShedItem> assets = JsonSerializer.Deserialize<List<ShedItem>>(json);

        return assets ?? new List<ShedItem>();
    }

    public async Task<List<ShedItem>> GetSHEDDataByNameWardsAndListingTypes(string name, List<string> ward, List<string> listingTypes)
    {
        string json = await _shedApiClient.GetSHEDDataByNameWardsAndListingTypes(name, ward, listingTypes);

        List<ShedItem> assets = JsonSerializer.Deserialize<List<ShedItem>>(json);

        return assets ?? new List<ShedItem>();
    }
}