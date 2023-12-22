using System.Net.Mime;
using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
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

        var processedDirectory = directoryHttpResponse.Content as ProcessedDirectory;

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = processedDirectory
        };

        if(processedDirectory.SubDirectories.Any())
            return View(directoryViewModel);

        return View("results", directoryViewModel);
    }

    [Route("/directories/results/{slug}")]
    public async Task<IActionResult> DirectoryResults(string slug)
    {
        var directoryHttpResponse = await _directoryRepository.Get<Directory>(slug);
        if (!directoryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        var processedDirectory = directoryHttpResponse.Content as ProcessedDirectory;

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = processedDirectory
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

        var processedDirectory = (ProcessedDirectory)directoryHttpResponse.Content;
        var kmlString = processedDirectory.ToKml();
        return Content(kmlString);
    }

    [Route("/directories/{slug}/directory-entry/{directoryEntrySlug}")]
    public async Task<IActionResult> DirectoryEntry(string slug, string directoryEntrySlug)
    {
        var directoryHttpResponse = await _directoryRepository.Get<Directory>(slug);
        var directoryEntryHttpResponse = await _directoryRepository.GetEntry<DirectoryEntry>(directoryEntrySlug);
        if (!directoryHttpResponse.IsSuccessful() || !directoryEntryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        var processedDirectory = directoryHttpResponse.Content as ProcessedDirectory;
        var processedEntryDirectory = directoryEntryHttpResponse.Content as ProcessedDirectoryEntry;

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = processedDirectory,
            DirectoryEntry = processedEntryDirectory
        };

        return View(directoryViewModel);
    }
}