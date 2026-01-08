using System.Text.Json;

namespace StockportWebapp.Repositories;

public interface IPublicationsTemplateRepository
{
    Task<HttpResponse> Get(string slug = "");
}

public class PublicationsTemplateRepository : IPublicationsTemplateRepository
{
    private readonly IHttpClient _httpClient;
    private readonly IStubToUrlConverter _urlGenerator;
    private readonly Dictionary<string, string> _authenticationHeaders;
    private readonly IApplicationConfiguration _config;

    public PublicationsTemplateRepository(
        UrlGenerator urlGenerator, 
        IHttpClient httpClient, 
        IApplicationConfiguration config)
    {
        _urlGenerator = urlGenerator;
        _httpClient = httpClient;
        _config = config;

        _authenticationHeaders = new Dictionary<string, string>
        {
            { "Authorization", _config.GetContentApiAuthenticationKey() },
            { "X-ClientId", _config.GetWebAppClientId() }
        };
    }

    public async Task<HttpResponse> Get(string slug = "")
    {
        string url = _urlGenerator.UrlFor<PublicationsTemplate>(slug);

        // Fetch the raw JSON from your API
        HttpResponse httpResponse = await _httpClient.Get(url, _authenticationHeaders);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        // Deserialize JSON into your frontend model
        try
        {
            var json = httpResponse.Content as string ?? string.Empty;

            if (string.IsNullOrWhiteSpace(json))
                return HttpResponse.Failure(500, "Empty response from API");

            var publicationsTemplate = System.Text.Json.JsonSerializer.Deserialize<PublicationsTemplate>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (publicationsTemplate == null)
                return HttpResponse.Failure(500, "Failed to deserialize PublicationsTemplate");

            return HttpResponse.Successful(200, publicationsTemplate);
        }
        catch (Exception ex)
        {
            return HttpResponse.Failure(500, $"Error deserializing PublicationsTemplate: {ex.Message}");
        }
    }
}