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

        return View(directoryViewModel);
    }

    [Route("/directories/kml/{slug}")]
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

    // [Route("/directories/{slug}/directory-entry/{directoryEntrySlug}")]
    public async Task<IActionResult> DirectoryEntryT(string directorySlug, string entrySlug, string subdirectoryPath)
    {
        var directoryHttpResponse = await _directoryRepository.Get<Directory>(directorySlug);
        var directoryEntryHttpResponse = await _directoryRepository.GetEntry<DirectoryEntry>(entrySlug);
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


    [Route("directory/{directorySlug}/{entrySlug}/{*subdirectoryPath}")]
    public async Task<IActionResult> DirectoryEntry(string directorySlug, string entrySlug, string subdirectoryPath="")
    {
        var directoryHttpResponse = await _directoryRepository.Get<Directory>(directorySlug);
        
        var directoryEntryHttpResponse = await _directoryRepository.GetEntry<DirectoryEntry>(entrySlug);
        if (!directoryHttpResponse.IsSuccessful() || !directoryEntryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        var processedDirectory = directoryHttpResponse.Content as ProcessedDirectory;
        var processedEntryDirectory = directoryEntryHttpResponse.Content as ProcessedDirectoryEntry;

        var subdirectories = subdirectoryPath?.Split('/') ?? Array.Empty<string>();
        List<Crumb> breadcrumbs = new()
        {
            new(processedDirectory.Title, processedDirectory.Slug, "Directories"),
            new(processedEntryDirectory.Name, processedEntryDirectory.Slug, "Directories")
        };

        // Fetch information for each subdirectory
        
        foreach (var subdirectory in subdirectories)
        {
            var subdirectoryHttpResponse = await _directoryRepository.Get<Directory>(subdirectory);

            if (subdirectoryHttpResponse.IsSuccessful())
            {
                var processedSubdirectory = subdirectoryHttpResponse.Content as ProcessedDirectory;
                // breadcrumbs.Add(
                //     new(processedSubdirectory.Title, processedSubdirectory.Slug, "Directories")
                // );

                DirectoryViewModel subdirectoryViewModel = new()
                {
                    Directory = processedDirectory,
                    DirectoryEntry = processedEntryDirectory,
                    SubDirectory = processedSubdirectory,
                    Breadcrumbs = breadcrumbs
                };

                return View("SubDirectory", subdirectoryViewModel);
            }
        }

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = processedDirectory,
            DirectoryEntry = processedEntryDirectory,
            Breadcrumbs = breadcrumbs
        };

        return View(directoryViewModel);
    }
}