namespace StockportWebapp.Client;

public interface ITPOApiClient
{
    Task<string> GetTPODataByID(string iD);
    
}

[ExcludeFromCodeCoverage]
public class TPOApiClient : ITPOApiClient
{
    private readonly System.Net.Http.HttpClient _httpClient;
    private readonly ILogger<TPOApiClient> _logger;

    public TPOApiClient(System.Net.Http.HttpClient httpClient, IApplicationConfiguration _config, ILogger<TPOApiClient> logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_config.GetTPOApiBaseUrl());
        _logger = logger;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.GetShedApiAuthToken());
    }

    public async Task<string> GetTPODataByID(string name)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"GetTPODataByID?id={Uri.EscapeDataString(name)}");

        if (!response.IsSuccessStatusCode)
            _logger.LogError($"{nameof(TPOApiClient)}::{nameof(GetTPODataByID)}: An error occurred fetching the TPO data: {response.StatusCode} - {response.ReasonPhrase}");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    
    
}