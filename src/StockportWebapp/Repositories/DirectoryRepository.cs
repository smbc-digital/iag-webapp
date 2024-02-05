using Directory = StockportWebapp.Models.Directory;
using Filter = StockportWebapp.Model.Filter;
namespace StockportWebapp.Repositories;

public interface IDirectoryRepository
{
    Task<HttpResponse> Get<T>(string slug = "");
    Task<HttpResponse> GetEntry<T>(string slug = "");
    IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory);
    IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory, string[] filters);
    IEnumerable<FilterTheme> GetAllFilterThemes(IEnumerable<DirectoryEntry> filteredEntries);
    IEnumerable<Filter> GetAppliedFilters(string[] filters, IEnumerable<FilterTheme> filterThemes);
    IEnumerable<DirectoryEntry> GetOrderedEntries(IEnumerable<DirectoryEntry> filteredEntries, string orderBy);
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

    public IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory) => 
        directory.AllEntries.Select(directoryEntry => directoryEntry).OrderBy(directoryEntry => directoryEntry.Name);

    public IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory, string[] filters) =>
        directory.AllEntries
            .Where(entry => entry is not null && entry.Themes is not null &&
                filters.All(filterSlug => entry.Themes
                    .Any(theme => theme is not null && theme.Filters is not null && theme.Filters
                    .Any(filter => filter.Slug.Equals(filterSlug)))))
            .ToList().OrderBy(directoryEntry => directoryEntry.Name);

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

    public IEnumerable<Filter> GetAppliedFilters(string[] filters, IEnumerable<FilterTheme> filterThemes) => 
        filters is not null && filters.Length > 0 && filterThemes is not null && filterThemes.Any()
            ? filterThemes
                .SelectMany(theme => theme.Filters)
                .Where(filter => filters.Contains(filter.Slug))
                .ToList()
            : new List<Filter>();

    public IEnumerable<DirectoryEntry> GetOrderedEntries(IEnumerable<DirectoryEntry> filteredEntries, string orderBy)
    {
        if(!string.IsNullOrEmpty(orderBy)) {
            if(orderBy.Equals("Name A to Z", StringComparison.OrdinalIgnoreCase))
                return filteredEntries.OrderBy(_ => _.Name);
            else if(orderBy.Equals("Name Z to A", StringComparison.OrdinalIgnoreCase))
                return filteredEntries.OrderByDescending(_ => _.Name);
        }

        return filteredEntries;
    }
}