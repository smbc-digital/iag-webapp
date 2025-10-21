namespace StockportWebapp.Client;

public interface IShedApiClient
{
    Task<string> GetSHEDDataByHeRef(string name);
    Task<string> GetSHEDDataByNameWardsTypeAndListingTypes(string name, List<string> ward, List<string> types, List<string> listingTypes);
}

[ExcludeFromCodeCoverage]
public class ShedApiClient : IShedApiClient
{
    private readonly System.Net.Http.HttpClient _httpClient;
    private readonly ILogger<ShedApiClient> _logger;

    public ShedApiClient(System.Net.Http.HttpClient httpClient, IApplicationConfiguration _config, ILogger<ShedApiClient> logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_config.GetShedApiBaseUrl());
        _logger = logger;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.GetShedApiAuthToken());
    }

    public async Task<string> GetSHEDDataByHeRef(string name)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"GetSHEDDataByHeRef?id={Uri.EscapeDataString(name)}");

        if (!response.IsSuccessStatusCode)
            _logger.LogError($"{nameof(ShedApiClient)}::{nameof(GetSHEDDataByHeRef)}: An error occurred fetching the SHED data: {response.StatusCode} - {response.ReasonPhrase}");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetSHEDDataByNameWardsTypeAndListingTypes(string name, List<string> ward, List<string> types, List<string> listingTypes)
    {
        List<string> queryParams = new();

        if (!string.IsNullOrWhiteSpace(name))
            queryParams.Add($"name={Uri.EscapeDataString(name)}");

        if (ward is not null && ward.Any())
            queryParams.AddRange(ward.Select(wardName => $"ward={Uri.EscapeDataString(wardName)}"));

        if (types is not null && types.Any())
            queryParams.AddRange(types.Select(type => $"types={Uri.EscapeDataString(type)}"));

        if (listingTypes is not null && listingTypes.Any())
            queryParams.AddRange(listingTypes.Select(listingType => $"listingTypes={Uri.EscapeDataString(listingType)}"));

        string url = "GetSHEDDataByNameWardsTypeAndListingTypes";
        if (queryParams.Any())
            url += "?" + string.Join("&", queryParams);

        HttpResponseMessage response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            _logger.LogError($"{nameof(ShedApiClient)}::{nameof(GetSHEDDataByNameWardsTypeAndListingTypes)}: An error occurred fetching the SHED data: {response.StatusCode} - {response.ReasonPhrase}");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}