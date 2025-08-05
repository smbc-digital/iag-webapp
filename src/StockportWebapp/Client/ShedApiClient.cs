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
        HttpResponseMessage response = await _httpClient.GetAsync($"GetSHEDData?ward={Uri.EscapeDataString(ward)}&listingType={Uri.EscapeDataString(listingType)}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetSHEDDataByID(string id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"GetSHEDDataByID?id={Uri.EscapeDataString(id)}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetSHEDDataByName(string id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"GetSHEDDataByName?id={Uri.EscapeDataString(id)}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}