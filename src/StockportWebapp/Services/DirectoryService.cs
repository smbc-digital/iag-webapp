using Directory = StockportWebapp.Models.Directory;
using Filter = StockportWebapp.Model.Filter;
namespace StockportWebapp.Services;

public interface IDirectoryService
{
    Task<Directory> Get<T>(string slug = "");
    Task<DirectoryEntry> GetEntry<T>(string slug = "");
    IEnumerable<DirectoryEntry> GetFilteredEntries(IEnumerable<DirectoryEntry> entries, string[] filters);
    IEnumerable<FilterTheme> GetFilterThemes(IEnumerable<DirectoryEntry> filteredEntries);
    IEnumerable<Filter> GetFilters(string[] filters, IEnumerable<FilterTheme> filterThemes);
    IEnumerable<DirectoryEntry> GetSearchedEntryForDirectories(IEnumerable<DirectoryEntry> entries, string searchTerm);
    IEnumerable<DirectoryEntry> GetOrderedEntries(IEnumerable<DirectoryEntry> filteredEntries, string orderBy);
    Dictionary<string, int> GetAllFilterCounts(IEnumerable<DirectoryEntry> allEntries);
}

public class DirectoryService : IDirectoryService {
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly IRepository _repository;

    public DirectoryService(MarkdownWrapper markdownWrapper, IRepository repository)
    {
        _markdownWrapper = markdownWrapper;
        _repository = repository;
    }

    public async Task<Directory> Get<T>(string slug = "")
    {
        var httpResponse = await _repository.Get<Directory>(slug);

        if (!httpResponse.IsSuccessful())
            return null;

        var directory = (Directory)httpResponse.Content;

        directory.Body = _markdownWrapper.ConvertToHtml(directory.Body ?? "");

        return directory;
    }

    public async Task<DirectoryEntry> GetEntry<T>(string slug = "")
    {
        var httpResponse = await _repository.Get<DirectoryEntry>(slug);

        if (!httpResponse.IsSuccessful())
            return null;

        var directoryEntry = (DirectoryEntry)httpResponse.Content;

        directoryEntry.Description = _markdownWrapper.ConvertToHtml(directoryEntry.Description ?? "");
        directoryEntry.Address = _markdownWrapper.ConvertToHtml(directoryEntry.Address ?? "");

        return directoryEntry;
    }

    public IEnumerable<DirectoryEntry> GetSearchedEntryForDirectories(IEnumerable<DirectoryEntry> entries, string searchTerm)
    {
        searchTerm = searchTerm.ToLower();
        
        return entries
            .Where(entry =>
                    (!string.IsNullOrEmpty(entry.Name) && entry.Name.ToLower().Contains(searchTerm))
                    || (!string.IsNullOrEmpty(entry.Teaser) && entry.Teaser.ToLower().Contains(searchTerm))
                    || (!string.IsNullOrEmpty(entry.Description) && entry.Description.ToLower().Contains(searchTerm))
                    || (entry.Tags is not null
                        && entry.Tags.Any(tag => !string.IsNullOrEmpty(tag)
                            && tag.ToLower().Contains(searchTerm)))
                    || (entry.Themes is not null
                        && entry.Themes.Any(theme => theme is not null
                                            && theme.Filters is not null
                                            && theme.Filters.Any(filter => filter is not null
                                                                    && !string.IsNullOrEmpty(filter.DisplayName)
                                                                    && filter.DisplayName.ToLower().Contains(searchTerm)))))
            .ToList()
            .OrderBy(directoryEntry => directoryEntry.Name);
    }

    public IEnumerable<DirectoryEntry> GetFilteredEntries(IEnumerable<DirectoryEntry> entries, string[] appliedFilters)
    {
        var allFilterThemes = GetFilterThemes(entries);
        var appliedThemes = GetFilters(appliedFilters, allFilterThemes)
                                .GetFilterThemesFromFilters();

        if(!appliedThemes.Any())
            return new List<DirectoryEntry>();

        return entries.Where(entry => entry.Themes is not null && entry.Themes.Any()
                            && appliedThemes.IsDirectoryEntryRelevant(entry));
    }

    /// <summary>
    /// Return a list of FilterThemes relevant to the List of directory entries provided
    /// </summary>
    /// <param name="filteredEntries"></param>
    /// <returns></returns>
    public IEnumerable<FilterTheme> GetFilterThemes(IEnumerable<DirectoryEntry> filteredEntries) => 
        filteredEntries is not null && filteredEntries.Any()
            ? filteredEntries
                .Where(entry => entry.Themes is not null)
                .SelectMany(entry => entry.Themes)
                .Where(themeTitle => !string.IsNullOrEmpty(themeTitle.Title))
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

    /// <summary>
    /// Returns a list of Filter Objects for the list array of filter names provided
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="filterThemes"></param>
    /// <returns></returns>
    public IEnumerable<Filter> GetFilters(string[] filters, IEnumerable<FilterTheme> filterThemes) => 
        filters is not null && filters.Length > 0 && filterThemes is not null && filterThemes.Any()
            ? filterThemes
                .SelectMany(theme => theme.Filters)
                .Where(filter => filters.Contains(filter.Slug))
                .ToList()
            : new List<Filter>();

    public IEnumerable<DirectoryEntry> GetOrderedEntries(IEnumerable<DirectoryEntry> filteredEntries, string orderBy)
    {
        if (orderBy.Equals("Name A to Z", StringComparison.OrdinalIgnoreCase) || orderBy.Equals("Name-A-to-Z", StringComparison.OrdinalIgnoreCase))
            return filteredEntries.OrderBy(_ => _.Name);
        else if (orderBy.Equals("Name Z to A", StringComparison.OrdinalIgnoreCase) || orderBy.Equals("Name-Z-to-A", StringComparison.OrdinalIgnoreCase))
            return filteredEntries.OrderByDescending(_ => _.Name);

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