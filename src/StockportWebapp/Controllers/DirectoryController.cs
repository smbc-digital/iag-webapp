using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class DirectoryController : Controller
{
    private readonly IDirectoryRepository _directoryRepository;
    private readonly IDirectoryEntryRepository _directoryEntryRepository;

    public DirectoryController(IDirectoryRepository directoryRepository, IDirectoryEntryRepository directoryEntryRepository)
    {
        _directoryRepository = directoryRepository;
        _directoryEntryRepository = directoryEntryRepository;
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

        return View(directoryViewModel);
    }

    [Route("/directories/{slug}/directory-entry/{directoryEntrySlug}")]
    public async Task<IActionResult> DirectoryEntry(string slug, string directoryEntrySlug)
    {
        var directoryHttpResponse = await _directoryRepository.Get<Directory>(slug);
        var directoryEntryHttpResponse = await _directoryEntryRepository.Get<DirectoryEntry>(directoryEntrySlug);
        if (!directoryHttpResponse.IsSuccessful())
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