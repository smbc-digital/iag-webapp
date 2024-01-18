using Directory = StockportWebapp.Models.Directory;
using Filter = StockportWebapp.Model.Filter;
namespace StockportWebapp.Repositories;

public interface IDirectoryRepository
{
    Task<HttpResponse> Get<T>(string slug = "");
    Task<HttpResponse> GetEntry<T>(string slug = "");
    IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory);
    IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory, string filters);
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

        return HttpResponse.Successful(200, directory);
    }

    public async Task<HttpResponse> GetEntry<T>(string slug = "")
    {
        var url = _urlGenerator.UrlFor<DirectoryEntry>(slug);
        var httpResponse = await _httpClient.Get(url, _authenticationHeaders);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        var model = HttpResponse.Build<DirectoryEntry>(httpResponse);
        var directoryEntry = (DirectoryEntry)model.Content;

        return HttpResponse.Successful(200, directoryEntry);
    }

    public IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory)
    {
        return directory.AllEntries.Select(directoryEntry => directoryEntry);
    }

    public IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory, string filters)
    {
        var allEntries = new List<DirectoryEntry>();
        foreach(var entry in directory.AllEntries)
        {
            if(entry is not null)
            {
                if (entry.Themes is not null)
                {
                    foreach (var theme in entry.Themes)
                    {
                        if (theme is not null)
                        {
                            foreach (var filter in theme.Filters)
                            {
                                if (filter.Slug.Equals(filters))
                                    allEntries.Add(entry);
                            }
                        }
                    }
                }
            }
        }

        var directoriesWithThemes = directory.AllEntries.Where(_ => _.Themes is not null);
        var t = directoriesWithThemes.Where(entry => entry.Themes.Any(theme => theme.Filters.Any(filter => filter.Slug == filters)));

        return allEntries;
    }
}