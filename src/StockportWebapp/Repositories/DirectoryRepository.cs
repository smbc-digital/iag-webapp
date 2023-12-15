using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.Repositories;

public interface IDirectoryRepository
{
    Task<HttpResponse> Get<T>(string slug = "");
}

public class DirectoryRepository : IDirectoryRepository
{
    private readonly DirectoryFactory _directoryFactory;
    private readonly UrlGenerator _urlGenerator;
    private readonly IHttpClient _httpClient;
    private readonly IApplicationConfiguration _config;
    private Dictionary<string, string> _authenticationHeaders;

    public DirectoryRepository(DirectoryFactory directoryFactory, UrlGenerator urlGenerator, IHttpClient httpClient, IApplicationConfiguration config)
    {
        _directoryFactory = directoryFactory;
        _urlGenerator = urlGenerator;
        _httpClient = httpClient;
        _config = config;
        _authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
    }

    public async Task<HttpResponse> Get<T>(string slug = "")
    {
        var url = _urlGenerator.UrlFor<Directory>(slug);
        var httpResponse = await _httpClient.Get(url, _authenticationHeaders);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        var model = HttpResponse.Build<Directory>(httpResponse);
        var directory = (Directory)model.Content;

        var processedModel = _directoryFactory.Build(directory);
        return HttpResponse.Successful(200, processedModel);
    }
}