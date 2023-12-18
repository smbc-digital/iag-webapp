namespace StockportWebapp.Repositories;

public interface IDirectoryEntryRepository
{
    Task<HttpResponse> Get<T>(string slug = "");
}

public class DirectoryEntryRepository : IDirectoryEntryRepository
{
    private readonly DirectoryEntryFactory _directoryEntryFactory;
    private readonly UrlGenerator _urlGenerator;
    private readonly IHttpClient _httpClient;
    private readonly IApplicationConfiguration _config;
    private Dictionary<string, string> _authenticationHeaders;

    public DirectoryEntryRepository(DirectoryEntryFactory directoryEntryFactory, UrlGenerator urlGenerator, IHttpClient httpClient, IApplicationConfiguration config)
    {
        _directoryEntryFactory = directoryEntryFactory;
        _urlGenerator = urlGenerator;
        _httpClient = httpClient;
        _config = config;
        _authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
    }

    public async Task<HttpResponse> Get<T>(string slug = "")
    {
        var url = _urlGenerator.UrlFor<DirectoryEntry>(slug);
        var httpResponse = await _httpClient.Get(url, _authenticationHeaders);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        var model = HttpResponse.Build<DirectoryEntry>(httpResponse);
        var directoryEntry = (DirectoryEntry)model.Content;

        var processedModel = _directoryEntryFactory.Build(directoryEntry);
        return HttpResponse.Successful(200, processedModel);
    }
}