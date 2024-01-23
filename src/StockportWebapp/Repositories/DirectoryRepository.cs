using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.Repositories;

public interface IDirectoryRepository
{
    Task<HttpResponse> Get<T>(string slug = "");
    Task<HttpResponse> GetEntry<T>(string slug = "");
    IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory);
    IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory, string[] filters);
    IEnumerable<FilterTheme> GetAllFilterThemes(IEnumerable<DirectoryEntry> filteredEntries);
}

public class DirectoryRepository : IDirectoryRepository
{
    
    private readonly UrlGenerator _urlGenerator;
    private readonly IHttpClient _httpClient;
    private readonly IApplicationConfiguration _config;
    private Dictionary<string, string> _authenticationHeaders;

    public DirectoryRepository(UrlGenerator urlGenerator, IHttpClient httpClient, IApplicationConfiguration config)
    {
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

    public IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory, string[] filters) =>
        directory.AllEntries
            .Where(entry => 
                entry != null && 
                entry.Themes != null &&
                filters.All(filterSlug => entry.Themes
                    .Any(theme => theme != null && theme.Filters != null && theme.Filters
                    .Any(filter => filter.Slug == filterSlug))))
            .ToList();

    public IEnumerable<FilterTheme> GetAllFilterThemes(IEnumerable<DirectoryEntry> filteredEntries) => 
        filteredEntries is not null && filteredEntries.Any()
            ? filteredEntries
                .Where(entry => entry.Themes is not null)
                .SelectMany(entry => entry.Themes)
                .GroupBy(theme => theme.Title, StringComparer.OrdinalIgnoreCase)
                .Select(group => new FilterTheme
                {
                    Title = group.Key,
                    Filters = group
                        .SelectMany(theme => theme.Filters)
                        .Distinct()
                        .GroupBy(filter => filter.Slug, StringComparer.OrdinalIgnoreCase)
                        .Select(filterGroup => filterGroup.First())
                        .ToList()
                })
                .OrderBy(theme => theme.Title)
                .ToList()
            : new List<FilterTheme>();
}