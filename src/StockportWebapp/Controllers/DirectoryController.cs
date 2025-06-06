using StockportWebapp.Comparers;
using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class DirectoryController(IDirectoryService directoryService) : Controller
{
    private readonly IDirectoryService _directoryService = directoryService;
    private readonly string _defaultUrlPrefix = "directories";

    [Route("/directories/{**slug}")]
    public async Task<IActionResult> Directory([Required]string slug)
    {
        PageLocation pageLocation = slug.ProcessAsWildcardSlug();
        Directory directory = await _directoryService.Get<Directory>(pageLocation.Slug);

        if (directory is null)
            return NotFound();

        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);
        DirectoryViewModel viewModel = new(slug, directory, GetBreadcrumbsForDirectories(directory, parentDirectories, false, false))
        {
            ParentDirectory = new DirectoryViewModel(parentDirectories.FirstOrDefault() ?? directory),
            FirstSubDirectory = new DirectoryViewModel(parentDirectories.ElementAtOrDefault(1) ?? directory)
        };
        
        if (viewModel.PrimaryItems.Items.Any())
            return View(viewModel);

        return RedirectToAction("DirectoryResults", new { slug });
    }

    [HttpGet]
    [Route("/directories/results/{**slug}")]
    public async Task<IActionResult> DirectoryResults([Required][FromRoute]string slug, string[] filters, string orderBy, string searchTerm, [FromQuery] int page, [FromQuery] bool showAll)
    {
        PageLocation pageLocation = slug.ProcessAsWildcardSlug();
        Directory directory = await _directoryService.Get<Directory>(pageLocation.Slug);

        if(directory is null)
            return NotFound();

        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);
        IEnumerable<DirectoryEntry> entries = GetSearchedFilteredSortedEntries(directory.AllEntries, filters, orderBy, searchTerm);
        IEnumerable<DirectoryEntry> pinnedEntries = entries.Where(entry => directory.PinnedEntries.Any(pinnedEntry => pinnedEntry.Slug.Equals(entry.Slug)));
        IEnumerable<DirectoryEntry> regularEntries;

        if (string.IsNullOrEmpty(orderBy))
            regularEntries = entries.Where(entry => directory.RegularEntries.Any(regularEntry => regularEntry.Slug.Equals(entry.Slug)))
                                    .OrderBy(directoryEntry => directoryEntry.Name);
        else
            regularEntries = entries.Where(entry => directory.RegularEntries.Any(regularEntry => regularEntry.Slug.Equals(entry.Slug)));

        IEnumerable<FilterTheme> allFilterThemes = _directoryService.GetFilterThemes(entries);

        DirectoryViewModel viewModel = new(slug, directory, GetBreadcrumbsForDirectories(directory, parentDirectories, false, true), pinnedEntries, regularEntries, page, showAll)
        {
            ParentDirectory = new DirectoryViewModel(parentDirectories.FirstOrDefault() ?? directory),
            FirstSubDirectory = new DirectoryViewModel(parentDirectories.ElementAtOrDefault(1) ?? directory),
            SearchTerm = searchTerm,
            AllFilterThemes = allFilterThemes,
            AppliedFilters = _directoryService.GetFilters(filters, allFilterThemes),
            FilterCounts = _directoryService.GetAllFilterCounts(entries.Distinct(new SlugComparer()).Select(entry => (DirectoryEntry)entry)),
            Order = !string.IsNullOrEmpty(orderBy) ? orderBy.Replace("-", " ") : orderBy
        };

        return View("results", viewModel);
    }

    private IEnumerable<DirectoryEntry> GetSearchedFilteredSortedEntries(IEnumerable<DirectoryEntry> entries, string[] filters, string orderBy, string searchTerm)
    {
        entries = filters.Any()
            ? _directoryService.GetFilteredEntries(entries, filters) 
            : entries;

        if (!string.IsNullOrEmpty(searchTerm))
            entries = _directoryService.GetSearchedEntryForDirectories(entries, searchTerm);

        if(!string.IsNullOrEmpty(orderBy))
            entries = _directoryService.GetOrderedEntries(entries, orderBy);

        return entries;
    }

    [Route("directories/entry/{**slug}")]
    public async Task<IActionResult> DirectoryEntry([Required]string slug)
    {
        PageLocation pageLocation = slug.ProcessAsWildcardSlug();
        DirectoryEntry directoryEntry = await _directoryService.GetEntry<DirectoryEntry>(pageLocation.Slug);

        if(directoryEntry is null)
            return NotFound();
        
        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);
        MapDetails mapDetails = new()
        {
            MapPosition = directoryEntry.MapPosition
        };

        return View(new DirectoryEntryViewModel(slug,
                                            directoryEntry,
                                            GetBreadcrumbsForDirectories(parentDirectories.FirstOrDefault(), parentDirectories, true, false),
                                            mapDetails));
    }

    private List<Crumb> GetBreadcrumbsForDirectories(Directory currentDirectory,
                                                    List<Directory> parentDirectories,
                                                    bool viewLastBreadcrumbAsResults = false,
                                                    bool addCurrentDirectoryBreadcrumb = false) 
    {
        List<Crumb> breadcrumbs = new();
        parentDirectories.ForEach(directory => breadcrumbs.Add(GetBreadcrumbForDirectory(directory, parentDirectories, viewLastBreadcrumbAsResults)));

        if (addCurrentDirectoryBreadcrumb)
            breadcrumbs.Add(GetBreadcrumbForCurrentDirectory(currentDirectory, parentDirectories));        

        return breadcrumbs;
    }
    
    private Crumb GetBreadcrumbForDirectory(Directory directory, IList<Directory> parentDirectories, bool viewLastBreadcrumbAsResults = false)
    {
        string relativeUrl = string.Join("/", parentDirectories
                                            .Take(parentDirectories.IndexOf(directory) + 1)
                                            .Select(_ => _.Slug));
        string url = string.Empty;

        if(parentDirectories.Any())
            url = directory.Equals(parentDirectories[^1]) && viewLastBreadcrumbAsResults
                    ? $"{_defaultUrlPrefix}/results/{relativeUrl}"
                    : $"{_defaultUrlPrefix}/{relativeUrl}";
        else 
            url = $"{_defaultUrlPrefix}/{relativeUrl}";

        return new Crumb(directory.Title, url, "Directories");
    }

    private Crumb GetBreadcrumbForCurrentDirectory(Directory directory, IList<Directory> parentDirectories)
    {
        string relativeUrl = string.Join("/", parentDirectories.Select(_ => _.Slug));
        string url = $"{_defaultUrlPrefix}/{relativeUrl}/{directory.Slug}"; 

        return new Crumb(directory.Title, url, "Directories");
    }

    private async Task<List<Directory>> GetParentDirectories(IEnumerable<string> parentSlugs)
    {
        List<Directory> parentDirectories = new();
        foreach (string directorySlug in parentSlugs)
        {
            Directory parent = await _directoryService.Get<Directory>(directorySlug);
            if (parent is not null)
                parentDirectories.Add(parent);
        }
        
        return parentDirectories;
    }
}