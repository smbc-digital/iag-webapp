using SharpKml.Dom.Atom;
using Directory = StockportWebapp.Models.Directory;
using Filter = StockportWebapp.Model.Filter;
namespace StockportWebapp.Services;

public interface IDirectoryService
{
    Task<Directory> Get<T>(string slug = "");
    Task<DirectoryEntry> GetEntry<T>(string slug = "");
    IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory);
    IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory, string[] filters);
    IEnumerable<FilterTheme> GetAllFilterThemes(IEnumerable<DirectoryEntry> filteredEntries);
    IEnumerable<Filter> GetAppliedFilters(string[] filters, IEnumerable<FilterTheme> filterThemes);
    IEnumerable<DirectoryEntry> GetOrderedEntries(IEnumerable<DirectoryEntry> filteredEntries, string orderBy);
    Dictionary<string, int> GetAllFilterCounts(IEnumerable<DirectoryEntry> allEntries);
}

public class DirectoryService : IDirectoryService {
    private readonly IApplicationConfiguration _config;
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly IRepository _repository;

    public DirectoryService(IApplicationConfiguration config, MarkdownWrapper markdownWrapper, IRepository repository)
    {
        _config = config;
        _markdownWrapper = markdownWrapper;
        _repository = repository;
    }

    public async Task<Directory> Get<T>(string slug = "")
    {
        var httpResponse = await _repository.Get<Directory>(slug);

        if (!httpResponse.IsSuccessful())
            throw new HttpRequestException($"HTTP request failed with status code {httpResponse.StatusCode}");

        return (Directory)httpResponse.Content;
    }

    public async Task<DirectoryEntry> GetEntry<T>(string slug = "")
    {
        var httpResponse = await _repository.Get<DirectoryEntry>(slug);

        if (!httpResponse.IsSuccessful())
            throw new HttpRequestException($"HTTP request failed with status code {httpResponse.StatusCode}");

        var directoryEntry = (DirectoryEntry)httpResponse.Content;

        directoryEntry.Description = _markdownWrapper.ConvertToHtml(directoryEntry.Description ?? "");
        directoryEntry.Address = _markdownWrapper.ConvertToHtml(directoryEntry.Address ?? "");

        return directoryEntry;
    }

    public IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory) => 
        directory.AllEntries.Select(directoryEntry => directoryEntry).OrderBy(directoryEntry => directoryEntry.Name);

    // public IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory, string[] filters) =>
    //     directory.AllEntries
    //         .Where(entry => entry is not null && entry.Themes is not null &&
    //             filters.All(filterSlug => entry.Themes
    //                 .Any(theme => theme is not null && theme.Filters is not null && theme.Filters
    //                 .Any(filter => filter.Slug.Equals(filterSlug)))))
    //         .ToList().OrderBy(directoryEntry => directoryEntry.Name);

    public IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory, string[] filters)
    {
        List<DirectoryEntry> entries = new();

        if (directory.AllEntries is not null)
        {
            foreach(var entry in directory.AllEntries)
            {
                if (entry.Themes is not null)
                {
                    foreach(var filterTheme in entry.Themes)
                    {
                        if(filterTheme.Filters is not null)
                        {
                            foreach(var filter in filterTheme.Filters)
                            {      
                                // how to differentiate between themes when comparing filters, ive tried using filterTheme.Title 
                                // this is doing an OR across all of them
                                if (filters.Any(_ => _.Equals(filter.Slug)))
                                {
                                    if(!entries.Contains(entry))
                                        entries.Add(entry);
                                }
                            }
                        }
                    }
                }
            }
        }

        return entries;
    }
    
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

    public Dictionary<string, int> GetAllFilterCounts(IEnumerable<DirectoryEntry> allEntries) =>
        allEntries
            .Where(entry => entry.Themes is not null)
            .SelectMany(entry => entry.Themes)
            .Where(theme => theme.Filters is not null)
            .SelectMany(theme => theme.Filters)
            .GroupBy(filter => filter.Slug)
            .ToDictionary(group => group.Key, group => group.Count());
}