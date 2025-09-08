using System.Net.Http.Headers;

namespace StockportWebapp.Client;

public class ShedApiClient
{
    private readonly System.Net.Http.HttpClient _httpClient;

    public ShedApiClient(System.Net.Http.HttpClient httpClient, IApplicationConfiguration _config)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_config.GetShedApiBaseUrl());
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.GetShedApiAuthToken());
    }

    public async Task<string> GetSHEDData(string ward, string listingType)
    {
        if (string.IsNullOrWhiteSpace(ward) && string.IsNullOrWhiteSpace(listingType))
            throw new ArgumentException("At least one of 'ward' or 'listingType' must be provided.");

        List<string> queryParams = new();

        if (!string.IsNullOrWhiteSpace(ward))
            queryParams.Add($"ward={Uri.EscapeDataString(ward)}");

        if (!string.IsNullOrWhiteSpace(listingType))
            queryParams.Add($"listingType={Uri.EscapeDataString(listingType)}");

        string queryString = string.Join("&", queryParams);
        string url = $"GetSHEDData?{queryString}";

        HttpResponseMessage response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetSHEDDataByID(string id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"GetSHEDDataByID?id={Uri.EscapeDataString(id)}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetSHEDDataByName(string name)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"GetSHEDDataByName?id={Uri.EscapeDataString(name)}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetAllSHEDData()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("GetAllSHEDData");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetSHEDDataByWardsAndListingTypes(List<string> ward, List<string> listingTypes)
    {
        var queryParams = new List<string>();

        if (ward != null && ward.Any())
            queryParams.AddRange(ward.Select(w => $"ward={Uri.EscapeDataString(w)}"));

        if (listingTypes != null && listingTypes.Any())
            queryParams.AddRange(listingTypes.Select(t => $"listingTypes={Uri.EscapeDataString(t)}"));

        string url = "GetSHEDDataByWardsAndListingTypes";
        if (queryParams.Any())
            url += "?" + string.Join("&", queryParams);

        HttpResponseMessage response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}