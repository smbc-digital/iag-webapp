using System.Net.Mime;
using Filter = StockportWebapp.Model.Filter;
using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class DirectoryController : Controller
{
    private readonly IDirectoryRepository _directoryRepository;
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly IFeatureManager _featureManager;

    public DirectoryController(IDirectoryRepository directoryRepository, MarkdownWrapper markdownWrapper, IFeatureManager featureManager = null)
    {
        _directoryRepository = directoryRepository;
        _markdownWrapper = markdownWrapper;
    }

    [Route("/directories/{slug}")]
    public async Task<IActionResult> Directory(string slug)
    {
        if (_featureManager is not null 
                && await _featureManager.IsEnabledAsync("Directories"))
                return NotFound();

        var directoryHttpResponse = await _directoryRepository.Get<Directory>(slug);
        if (!directoryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        var directory = directoryHttpResponse.Content as Directory;

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = directory,
            FilteredEntries = _directoryRepository.GetFilteredEntryForDirectories(directory)
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

        var directoryHttpResponse = await _directoryRepository.Get<Directory>(slug);
        
        if (!directoryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        var directory = directoryHttpResponse.Content as Directory;

        var filteredEntries =  filters.Any() 
            ? _directoryRepository.GetFilteredEntryForDirectories(directory, filters) 
            : _directoryRepository.GetFilteredEntryForDirectories(directory);

        var allFilterThemes = _directoryRepository.GetAllFilterThemes(filteredEntries);
        var appliedFilters = _directoryRepository.GetAppliedFilters(filters, allFilterThemes);
        
        filteredEntries = _directoryRepository.GetOrderedEntries(filteredEntries, orderBy);

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = directory,
            FilteredEntries = filteredEntries,
            AllFilterThemes = allFilterThemes,
            AppliedFilters = appliedFilters,
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

        var directoryHttpResponse = await _directoryRepository.Get<Directory>(slug);
        if (!directoryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        var directory = (Directory)directoryHttpResponse.Content;
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

        var directoryHttpResponse = await _directoryRepository.Get<Directory>(directorySlug);
        var directoryEntryHttpResponse = await _directoryRepository.GetEntry<DirectoryEntry>(entrySlug);
        if (!directoryEntryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        var directory = directoryHttpResponse.Content as Directory;
        var processedDirectoryEntry = directoryEntryHttpResponse.Content as DirectoryEntry;
        processedDirectoryEntry.Description = _markdownWrapper.ConvertToHtml(processedDirectoryEntry.Description ?? "");
        processedDirectoryEntry.Address = _markdownWrapper.ConvertToHtml(processedDirectoryEntry.Address ?? "");

        return View(new DirectoryViewModel()
        {
            Directory = directory,
            DirectoryEntry = processedDirectoryEntry
        });
    }
}