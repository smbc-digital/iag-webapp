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

        var pageLocation = slug.ProcessAsWildcardSlug();

        var directory = await _directoryService.Get<Directory>(pageLocation.Slug);
        if (directory is null)
            return NotFound();
        
        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);
        
        DirectoryViewModel directoryViewModel = new() {
            Directory = directory,
            ParentDirectory = parentDirectories.FirstOrDefault() ?? directory,
            FirstSubDirectory = parentDirectories.ElementAtOrDefault(1) ?? directory,
            Breadcrumbs = GetBreadcrumbsForDirectories(directory, parentDirectories, false, false),
            Slug = slug,
        };

        if (directory.SubDirectories.Any() && directory.SubDirectories is not null && directory.SubDirectories.Any(subdir => subdir is not null))
            return View(directoryViewModel);

        return RedirectToAction("DirectoryResults", new { slug });
    }

    [HttpGet]
    [Route("/directories/results/{**slug}")]
    public async Task<IActionResult> DirectoryResults([Required][FromRoute]string slug, string[] filters, string orderBy, string searchTerm, [FromQuery] int page)
    {
        if (!_isToggledOn || string.IsNullOrEmpty(slug))
            return NotFound();

        var pageLocation = slug.ProcessAsWildcardSlug();

        var directory = await _directoryService.Get<Directory>(pageLocation.Slug);
        if(directory is null)
            return NotFound();

        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);

        var regularEntries = directory.AllEntries.Where(entry => !directory.PinnedEntries.Any(pinnedEntry => pinnedEntry.Slug.Equals(entry.Slug)));
        var entries = GetSearchedFilteredSortedEntries(regularEntries.Concat(directory.PinnedEntries), filters, orderBy, searchTerm);
        var pinnedEntries = entries.Where(entry => directory.PinnedEntries.Any(pinnedEntry => pinnedEntry.Slug.Equals(entry.Slug)));
        var allFilterThemes = _directoryService.GetFilterThemes(entries.Concat(pinnedEntries));
        var viewModel = new DirectoryViewModel
        {
            Slug = slug,
            Breadcrumbs = GetBreadcrumbsForDirectories(directory, parentDirectories, false, true),
            Directory = directory,
            ParentDirectory = parentDirectories.FirstOrDefault() ?? directory,
            FirstSubDirectory = parentDirectories.ElementAtOrDefault(1) ?? directory,
            FilteredEntries = entries,
            AllFilterThemes = allFilterThemes,
            PinnedEntries = pinnedEntries,
            AppliedFilters = _directoryService.GetFilters(filters, allFilterThemes),
            FilterCounts = _directoryService.GetAllFilterCounts(entries.Concat(pinnedEntries).Distinct(new DirectoryEntryComparer())),
            SearchTerm = searchTerm,
            Order = !string.IsNullOrEmpty(orderBy) ? orderBy.Replace("-", " ") : orderBy
        };
        
        DirectoryViewModel.DoPagination(viewModel, page);

        return View("results", viewModel);
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

        var pageLocation = slug.ProcessAsWildcardSlug();
        var directoryEntry = await _directoryService.GetEntry<DirectoryEntry>(pageLocation.Slug);
        
        if(directoryEntry is null)
            return NotFound();
        
        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);

        return View(new DirectoryViewModel()
        {
            Directory = parentDirectories.FirstOrDefault(),
            DirectoryEntry = directoryEntry,
            Breadcrumbs = GetBreadcrumbsForDirectories(parentDirectories.FirstOrDefault(), parentDirectories, true, false),
            Slug = slug
        });
    }

    private List<Crumb> GetBreadcrumbsForDirectories(Directory currentDirectory, List<Directory> parentDirectories, bool viewLastBreadcrumbAsResults = false, bool addCurrentDirectoryBreadcrumb = false) 
    {
        
        List<Crumb> breadcrumbs = new();
        parentDirectories.ForEach(directory => breadcrumbs.Add(GetBreadcrumbForDirectory(directory, parentDirectories, viewLastBreadcrumbAsResults)));

        if (addCurrentDirectoryBreadcrumb)
            breadcrumbs.Add(GetBreadcrumbForCurrentDirectory(currentDirectory, parentDirectories));        

        return breadcrumbs;
    }
    
    private Crumb GetBreadcrumbForDirectory(Directory directory, IList<Directory> parentDirectories, bool viewLastBreadcrumbAsResults = false)
    {
        var relativeUrl = string.Join("/", parentDirectories
                                            .Take(parentDirectories.IndexOf(directory) + 1)
                                            .Select(_ => _.Slug));
        var url = "";
        if(parentDirectories.Any())
        {
            url = directory.Equals(parentDirectories[^1]) && viewLastBreadcrumbAsResults
            ? $"{_defaultUrlPrefix}/results/{relativeUrl}"
            : $"{_defaultUrlPrefix}/{relativeUrl}";
        }
        else 
        {
            url = $"{_defaultUrlPrefix}/{relativeUrl}";
        }

        return new Crumb(directory.Title, url, "Directories");
    }

    private Crumb GetBreadcrumbForCurrentDirectory(Directory directory, IList<Directory> parentDirectories)
    {
        var relativeUrl = string.Join("/", parentDirectories.Select(_ => _.Slug));

        var url = $"{_defaultUrlPrefix}/{relativeUrl}/{directory.Slug}"; 

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