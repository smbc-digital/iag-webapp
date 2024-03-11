using System.Net.Mime;
using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class DirectoryController : Controller
{
    private readonly IDirectoryService _directoryService;
    private readonly IFeatureManager _featureManager;
    private readonly bool _isToggledOn = true;
    private readonly string _defaultUrlPrefix = "directories";

    public DirectoryController(IDirectoryService directoryService, IFeatureManager featureManager = null)
    {
        _featureManager = featureManager;

        if (_featureManager is not null)
            _isToggledOn = _featureManager.IsEnabledAsync("Directories").Result;

        _directoryService = directoryService;
    }

    [Route("/directories/{**slug}")]
    public async Task<IActionResult> Directory(string slug)
    {
        if (!_isToggledOn || string.IsNullOrEmpty(slug))
            return NotFound();

        var pageLocation = WildcardExtensions.ProcessSlug(slug);

        var directory = await _directoryService.Get<Directory>(pageLocation.Slug);
        if (directory is null)
            return NotFound();
        
        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);
        
        DirectoryViewModel directoryViewModel = new() {
            Directory = directory,
            ParentDirectory = parentDirectories.FirstOrDefault() ?? directory,
            FirstSubDirectory = parentDirectories.ElementAtOrDefault(1) ?? directory,
            Breadcrumbs = GetBreadcrumbsForDirectories(parentDirectories, false),
            Slug = slug,
        };

        if (directory.SubDirectories.Any())
            return View(directoryViewModel);

        return RedirectToAction("DirectoryResults", new { slug });
    }

    [HttpGet]
    [Route("/directories/results/{**slug}")]
    public async Task<IActionResult> DirectoryResults([Required][FromRoute]string slug, string[] filters, string orderBy, string searchTerm)
    {
        if (!_isToggledOn || string.IsNullOrEmpty(slug))
            return NotFound();

        var pageLocation = WildcardExtensions.ProcessSlug(slug);

        var directory = await _directoryService.Get<Directory>(pageLocation.Slug);
        if(directory is null)
            return NotFound();

        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);

        var entries = GetSearchedFilteredSortedEntries(directory.AllEntries, filters, orderBy, searchTerm);
        var allFilterThemes = _directoryService.GetFilterThemes(entries);
        
        return View("results", new DirectoryViewModel
        {
            Slug = slug,
            Breadcrumbs = GetBreadcrumbsForDirectories(parentDirectories, false),
            Directory = directory,
            FilteredEntries = entries,
            AllFilterThemes = allFilterThemes,
            AppliedFilters = _directoryService.GetFilters(filters, allFilterThemes),
            FilterCounts = _directoryService.GetAllFilterCounts(entries),
            SearchTerm = searchTerm,
            Order = orderBy
        });
    }

    private IEnumerable<DirectoryEntry> GetSearchedFilteredSortedEntries(IEnumerable<DirectoryEntry> entries, string[] filters, string orderBy, string searchTerm)
    {
        entries = filters.Any()
            ? _directoryService.GetFilteredEntries(entries, filters) 
            : entries.OrderBy(directoryEntry => directoryEntry.Name);

        if (!string.IsNullOrEmpty(searchTerm))
            entries = _directoryService.GetSearchedEntryForDirectories(entries, searchTerm);

        if(!string.IsNullOrEmpty(orderBy))
            entries = _directoryService.GetOrderedEntries(entries, orderBy);

        return entries;
    }

    [Route("/directories/kml/{slug}")]  
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> DirectoryAsKml([Required][FromRoute]string slug, string[] filters, string orderBy, string searchTerm)
    {
        if (!_isToggledOn)
            return NotFound();

        var directory = await _directoryService.Get<Directory>(slug);
        if(directory is null)
            return NotFound();

        var entries = GetSearchedFilteredSortedEntries(directory.AllEntries, filters, orderBy, searchTerm);

        var kmlString = entries.GetKmlForList();
        return Content(kmlString);
    }

    [Route("directories/entry/{**slug}")]
    public async Task<IActionResult> DirectoryEntry(string slug)
    {
        if (!_isToggledOn || string.IsNullOrEmpty(slug))
            return NotFound();

        var pageLocation = WildcardExtensions.ProcessSlug(slug);
        var directoryEntry = await _directoryService.GetEntry<DirectoryEntry>(pageLocation.Slug);
        
        if(directoryEntry is null)
            return NotFound();
        
        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);

        return View(new DirectoryViewModel()
        {
            Directory = parentDirectories.FirstOrDefault(),
            DirectoryEntry = directoryEntry,
            Breadcrumbs = GetBreadcrumbsForDirectories(parentDirectories, true),
            Slug = slug
        });
    }

    private List<Crumb> GetBreadcrumbsForDirectories(List<Directory> parentDirectories, bool viewLastBreadcrumbAsResults = false) 
    {
        List<Crumb> breadcrumbs = new();
        parentDirectories.ForEach(directory => breadcrumbs.Add(GetBreadcrumbForDirectory(directory, parentDirectories, viewLastBreadcrumbAsResults)));
        return breadcrumbs;
    }

    private Crumb GetBreadcrumbForDirectory(Directory directory, IList<Directory> parentDirectories, bool viewLastBreadcrumbAsResults = false)
    {
        var relativeUrl = string.Join("/", parentDirectories
                                            .Take(parentDirectories.IndexOf(directory) + 1)
                                            .Select(_ => _.Slug));

        var url = directory.Equals(parentDirectories[^1]) && viewLastBreadcrumbAsResults
            ? $"{_defaultUrlPrefix}/results/{relativeUrl}"
            : $"{_defaultUrlPrefix}/{relativeUrl}";

        return new Crumb(directory.Title, url, "Directories");
    }

    private async Task<List<Directory>> GetParentDirectories(IEnumerable<string> parentSlugs)
    {
        List<Directory> parentDirectories = new();
        foreach (var directorySlug in parentSlugs)
        {
            var parent = await _directoryService.Get<Directory>(directorySlug);
            if (parent is not null)
                parentDirectories.Add(parent);
        }
        return parentDirectories;
    }
}