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
        var entries = new List<DirectoryEntry>();

        var allFilterThemes = GetAllFilterThemes(directory.AllEntries);
        var appliedFilters = GetAppliedFilters(filters, allFilterThemes);

        // filterConditions dict based on applied filters, so we don't have to loop too many times
        var filterConditions = new Dictionary<string, List<string>>();
        foreach (var filter in appliedFilters)
        {
            if (!filterConditions.ContainsKey(filter.Theme))
                filterConditions[filter.Theme] = new List<string>();
            
            filterConditions[filter.Theme].Add(filter.Slug);
        }

        // filter directory entries based on filterConditions and applied filters
        var filteredEntries = directory.AllEntries.Where(entry =>
        {
            if (entry.Themes is null)
                return false;

            // Check if the entry satisfies all filter conditions
            return filterConditions.All(condition =>
            {
                var themeFilters = condition.Value; // Get filters for the current theme
                var appliedThemeFilters = appliedFilters
                    .Where(appliedFilter => themeFilters.Contains(appliedFilter.Slug)); // Applied filters relevant to the current theme

                // Check if the entry satisfies filter conditions for the current theme
                return appliedThemeFilters.Any(appliedFilter =>
                    entry.Themes.Any(theme =>
                        theme.Title == condition.Key && theme.Filters.Any(filter =>
                            filter.Slug == appliedFilter.Slug)));
            });
        }).ToList();

        return filteredEntries;
    }


    // public IEnumerable<DirectoryEntry> GetFilteredEntryForDirectories(Directory directory, string[] filters)
    // {
    //     var entries = new List<DirectoryEntry>();

    //     var allFilterThemes = GetAllFilterThemes(directory.AllEntries);
    //     var appliedFilters = GetAppliedFilters(filters, allFilterThemes);
    //     var groupedFilters = appliedFilters.GroupBy(filter => filter.Theme);
        
    //     var relevantEntries = new List<List<DirectoryEntry>>();

    //     foreach(var group in groupedFilters)
    //         relevantEntries.Add(GetFilteredEntriesForFilterGroup(directory, group).ToList());

    //     entries = relevantEntries.FirstOrDefault();

    //     foreach(var groupEntries in relevantEntries.Skip(1))
    //         entries = entries.Intersect(groupEntries).ToList();

    //     return entries.Distinct(new DirectoryEntryComparer());
    // }

    private IEnumerable<DirectoryEntry> GetFilteredEntriesForFilterGroup(Directory directory, IGrouping<string, Filter> group)
    {
        var entries = new List<DirectoryEntry>();

        foreach(var filter in group)
        {
            var result = directory.AllEntries
                .Where(entry => entry is not null && entry.Themes is not null 
                && entry.Themes.SelectMany(theme => theme.Filters)
                .Any(f => f.Slug.Equals(filter.Slug)));

            entries = entries.Concat(result).ToList();
        }

        return entries.Distinct(new DirectoryEntryComparer());
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