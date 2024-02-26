using System.Net.Mime;
using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class DirectoryController : Controller
{
    private readonly IDirectoryService _directoryService;
    private readonly IFeatureManager _featureManager;

    public DirectoryController(IDirectoryService directoryService, 
        IFeatureManager featureManager = null)
    {
        _directoryService = directoryService;
    }

    [Route("/directories/{**slug}")]
    public async Task<IActionResult> Directory(string slug)
    {
        if (_featureManager is not null 
            && await _featureManager.IsEnabledAsync("Directories"))
                return NotFound();

        var pageLocation = ProcessWildcardSlug(slug);

        var directory = await _directoryService.Get<Directory>(pageLocation.Slug);
        if(directory is null)
            return NotFound();
        
        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);

        DirectoryViewModel directoryViewModel = new() {
            Directory = directory,
            Breadcrumbs = GetBreadcrumbsForDirectories(parentDirectories, false),
            Slug = slug,
            FilteredEntries = _directoryService.GetFilteredEntryForDirectories(directory)
        };

        if(directory.SubDirectories.Any())
            return View(directoryViewModel);
            
        directoryViewModel.AllFilterThemes = _directoryService.GetAllFilterThemes(directoryViewModel.FilteredEntries);
        directoryViewModel.FilterCounts = _directoryService.GetAllFilterCounts(directory.AllEntries);

        return View("results", directoryViewModel);
    }

    [HttpGet]
    [Route("/directories/results/{**slug}")]
    public async Task<IActionResult> DirectoryResults([Required][FromRoute]string slug, string[] filters, string orderBy)
    {
        if (_featureManager is not null
        && await _featureManager.IsEnabledAsync("Directories"))
            return NotFound();

        var pageLocation = ProcessWildcardSlug(slug);
        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);

        var directory = await _directoryService.Get<Directory>(pageLocation.Slug);
        if(directory is null)
            return NotFound();

        var filteredEntries = filters.Any()
            ? _directoryService.GetFilteredEntryForDirectories(directory, filters)
            : _directoryService.GetFilteredEntryForDirectories(directory);

        var allFilterThemes = _directoryService.GetAllFilterThemes(filteredEntries);
        var appliedFilters = _directoryService.GetAppliedFilters(filters, allFilterThemes);

        filteredEntries = _directoryService.GetOrderedEntries(filteredEntries, orderBy);

        var filterCounts = filters.Any()
            ? _directoryService.GetAllFilterCounts(filteredEntries)
            : _directoryService.GetAllFilterCounts(directory.AllEntries);

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = directory,
            FilteredEntries = filteredEntries,
            AllFilterThemes = allFilterThemes,
            AppliedFilters = appliedFilters,
            FilterCounts = filterCounts,
            Order = orderBy,
            Breadcrumbs = GetBreadcrumbsForDirectories(parentDirectories, false),
            Slug = slug
        };
        
        return View("results", directoryViewModel);
    }


    [Route("/directories/results/kml/{slug}")]  
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> DirectoryAsKml(string slug)
    {
        if (_featureManager is not null
        && await _featureManager.IsEnabledAsync("Directories"))
            return NotFound();

        var directory = await _directoryService.Get<Directory>(slug);
        if(directory is null)
            return NotFound();

        var kmlString = directory.ToKml();
        return Content(kmlString);
    }
    
    [Route("directories/entry/{**slug}")]
    public async Task<IActionResult> DirectoryEntry(string slug)
    {
        var pageLocation = ProcessWildcardSlug(slug);

        if (_featureManager is not null
        && await _featureManager.IsEnabledAsync("Directories"))
            return NotFound();

        var directoryEntry = await _directoryService.GetEntry<DirectoryEntry>(pageLocation.Slug);
        
        if(directoryEntry is null)
            return NotFound();
        
        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);

        return View(new DirectoryViewModel()
        {
            Directory = parentDirectories.First(),
            DirectoryEntry = directoryEntry,
            Breadcrumbs = GetBreadcrumbsForDirectories(parentDirectories, true),
            Slug = slug
        });
    }

    private PageLocation ProcessWildcardSlug(string slug)
    {
        var slugValues = slug?.Split('/') ?? Array.Empty<string>();
        return new PageLocation(slugValues.Last(), slugValues.SkipLast(1).ToList());
    }

    private List<Crumb> GetBreadcrumbsForDirectories(IList<Directory> parentDirectories, bool viewLastBreadcrumbAsResults = false) 
    {
        List<Crumb> breadcrumbs = new();
        for (int i = 0; i < parentDirectories.Count; i++)
        {
            var directory = parentDirectories[i];
            var relativeUrl = string.Join("/", parentDirectories.Take(i + 1).Select(_ => _.Slug));
            if(i == parentDirectories.Count - 1 && viewLastBreadcrumbAsResults)
            {
                breadcrumbs.Add(new Crumb(directory.Title, $"directories/results/{relativeUrl}", "Directories"));
            }
            else
            {
                breadcrumbs.Add(new Crumb(directory.Title, $"directories/{relativeUrl}", "Directories"));
            }
            
        }

        return breadcrumbs;
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

public class PageLocation
{
    public string Slug { get; private set; }
    public IEnumerable<string> ParentSlugs { get; private set; }

    public PageLocation(string slug, IEnumerable<string> parentSlugs)
    {
        Slug = slug;
        ParentSlugs = parentSlugs;
    }
}