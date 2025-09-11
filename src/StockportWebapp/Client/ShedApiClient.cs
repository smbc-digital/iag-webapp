namespace StockportWebapp.Client;

public interface IShedApiClient
{
    Task<string> GetSHEDDataByName(string name);
    Task<string> GetSHEDDataByNameWardsAndListingTypes(string name, List<string> ward, List<string> listingTypes);
}

[ExcludeFromCodeCoverage]
public class ShedApiClient : IShedApiClient
{
    private readonly System.Net.Http.HttpClient _httpClient;

    public ShedApiClient(System.Net.Http.HttpClient httpClient, IApplicationConfiguration _config)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_config.GetShedApiBaseUrl());
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.GetShedApiAuthToken());
    }

    public async Task<string> GetSHEDDataByName(string name)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"GetSHEDDataByName?id={Uri.EscapeDataString(name)}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetSHEDDataByNameWardsAndListingTypes(string name, List<string> ward, List<string> listingTypes)
    {
        List<string> queryParams = new();

        if (!string.IsNullOrWhiteSpace(name))
            queryParams.Add($"name={Uri.EscapeDataString(name)}");

        if (ward is not null && ward.Any())
            queryParams.AddRange(ward.Select(w => $"ward={Uri.EscapeDataString(w)}"));

        if (listingTypes is not null && listingTypes.Any())
            queryParams.AddRange(listingTypes.Select(t => $"listingTypes={Uri.EscapeDataString(t)}"));

        string url = "GetSHEDDataByNameWardsAndListingTypes";
        if (queryParams.Any())
            url += "?" + string.Join("&", queryParams);

        HttpResponseMessage response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}