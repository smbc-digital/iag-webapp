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

    [Route("directories/entry/{**slug}")]
    public async Task<IActionResult> DirectoryEntry(string slug)
    {
        // Get slug values from catch all
        var slugValues = slug?.Split('/') ?? Array.Empty<string>();

        // Last value will always be the entry slug
        var entrySlug = slugValues.Last();

        // Get the requested entry
        var directoryEntryHttpResponse = await _directoryRepository.GetEntry<DirectoryEntry>(entrySlug);
        if (!directoryEntryHttpResponse.IsSuccessful())
            return directoryEntryHttpResponse;

        var processedEntryDirectory = directoryEntryHttpResponse.Content as ProcessedDirectoryEntry;

        // Anything before the (last) entry slug is directory hierarchy
        var directorySlugs = slugValues.SkipLast(1).ToList();
        List<ProcessedDirectory> parentDirectories = new();

        // Get the parent directories
        HttpResponse directoryHttpResponse;
        foreach (var directorySlug in directorySlugs)
        {
            directoryHttpResponse = await _directoryRepository.Get<Directory>(directorySlug);
            if (!directoryHttpResponse.IsSuccessful())
                return directoryEntryHttpResponse;

            var p = directoryHttpResponse.Content as ProcessedDirectory;
            parentDirectories.Add(p);
        }

        // Create breadcrumbs for the parent
        List<Crumb> breadcrumbs = new();
        for (int i =0; i < parentDirectories.Count; i++)
        {
            var directory = parentDirectories[i];
            var relativeParentDirectories = parentDirectories.GetRange(0, i);
            var directorySlug = string.Join("/", relativeParentDirectories);
            breadcrumbs.Add(new(directory.Title, directorySlug, "Directories"));
        }

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = parentDirectories.First(),
            DirectoryEntry = processedEntryDirectory,
            Breadcrumbs = breadcrumbs
        };

        return View(directoryViewModel);
    }
}