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

    [Route("/directories/{slug}")]
    public async Task<IActionResult> Directory(string slug)
    {
        if (_featureManager is not null 
            && await _featureManager.IsEnabledAsync("Directories"))
                return NotFound();

        var directory = await _directoryService.Get<Directory>(slug);
        if(directory is null)
            return NotFound();

        DirectoryViewModel directoryViewModel = new() {
            Directory = directory,
            FilteredEntries = _directoryService.GetFilteredEntryForDirectories(directory)
        };

        if(directory.SubDirectories.Any())
            return View(directoryViewModel);

        return View("results", directoryViewModel);
    }
    
    [HttpGet]
    [Route("/directories/results/{slug}")]
    public async Task<IActionResult> DirectoryResults([Required][FromRoute]string slug, string[] filters, string orderBy)
    {
        if (_featureManager is not null
        && await _featureManager.IsEnabledAsync("Directories"))
            return NotFound();

        var directory = await _directoryService.Get<Directory>(slug);
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
            Order = orderBy
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

    // [Route("/directories/{slug}/directory-entry/{directoryEntrySlug}")]
    [Route("/directory-entry/{entrySlug}")]
    [Route("/directories/entry/{directorySlug}/{entrySlug}")]
    public async Task<IActionResult> DirectoryEntry(string directorySlug, string entrySlug)
    {
        if (_featureManager is not null
        && await _featureManager.IsEnabledAsync("Directories"))
            return NotFound();

        var directory = await _directoryService.Get<Directory>(directorySlug);
        var directoryEntry = await _directoryService.GetEntry<DirectoryEntry>(entrySlug);
        
        if(directoryEntry is null)
            return NotFound();
        
        return View(new DirectoryViewModel()
        {
            Directory = directory,
            DirectoryEntry = directoryEntry
        });
    }
}