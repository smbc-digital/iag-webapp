namespace StockportWebapp.Repositories;

public interface IPublicationTemplateRepository
{
    Task<HttpResponse> Get(string slug = "");
}

public class PublicationTemplateRepository : IPublicationTemplateRepository
{
    private readonly IHttpClient _httpClient;
    private readonly IStubToUrlConverter _urlGenerator;
    private readonly Dictionary<string, string> _authenticationHeaders;
    private readonly IApplicationConfiguration _config;

    public PublicationTemplateRepository(
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
        string url = _urlGenerator.UrlFor<PublicationTemplate>(slug);

        HttpResponse httpResponse = await _httpClient.Get(url, _authenticationHeaders);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        try
        {
            string json = httpResponse.Content as string ?? string.Empty;

            if (string.IsNullOrWhiteSpace(json))
                return HttpResponse.Failure(500, "Empty response from API");

            PublicationTemplate publicationTemplate = System.Text.Json.JsonSerializer.Deserialize<PublicationTemplate>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (publicationTemplate is null)
                return HttpResponse.Failure(500, "Failed to deserialize PublicationTemplate");

            return HttpResponse.Successful(200, publicationTemplate);
        }
        catch (Exception ex)
        {
            return HttpResponse.Failure(500, $"Error deserializing PublicationTemplate: {ex.Message}");
        }
    }
}