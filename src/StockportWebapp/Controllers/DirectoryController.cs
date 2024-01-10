using System.Net.Mime;
using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class DirectoryController : Controller
{
    private readonly IDirectoryRepository _directoryRepository;

    public DirectoryController(IDirectoryRepository directoryRepository)
    {
        _directoryRepository = directoryRepository;
    }

    [Route("/directories/{slug}")]

    public async Task<IActionResult> Directory(string slug)
    {
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
    public async Task<IActionResult> DirectoryResults([Required][FromRoute]string slug, string parentSlug1, string parentSlug2, string parentSlug3, string parentSlug4)
    {
        var directoryHttpResponse = await _directoryRepository.Get<Directory>(slug);
        
        if (!directoryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        var directory = directoryHttpResponse.Content as Directory;

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = directory,
            FilteredEntries = _directoryRepository.GetFilteredEntryForDirectories(directory)
        };        
        
        return View("results", directoryViewModel);
    }

    [HttpPost]
    [Route("/directories/results/{slug}")]
    public async Task<IActionResult> FilterResults(string slug)
    {
        var directoryHttpResponse = await _directoryRepository.Get<Directory>(slug);
        if (!directoryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        var directory = directoryHttpResponse.Content as Directory;

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = directory,
            FilteredEntries = _directoryRepository.GetFilteredEntryForDirectories(directory)
        };

        return View("results", directoryViewModel);
    }

    [Route("/directories/results/kml/{slug}")]  
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> DirectoryAsKml(string slug)
    {
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
        var directoryHttpResponse = await _directoryRepository.Get<Directory>(directorySlug);
         var directoryEntryHttpResponse = await _directoryRepository.GetEntry<DirectoryEntry>(entrySlug);
        if (!directoryEntryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        var directory = directoryHttpResponse.Content as Directory;
        var processedDirectoryEntry = directoryEntryHttpResponse.Content as DirectoryEntry;

        return View(new DirectoryViewModel()
        {
            Directory = directory,
            DirectoryEntry = processedDirectoryEntry
        });
    }
}