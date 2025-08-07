namespace StockportWebapp.Services;

using System.Text.Json;
using StockportWebapp.Client;

public class ShedService(ShedApiClient shedApiClient)
{
    private readonly ShedApiClient _shedApiClient = shedApiClient;

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

        return assets ?? new List<ShedItem>();
    }
}