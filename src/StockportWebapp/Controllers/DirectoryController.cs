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
    [Route("/directories/results/{parentSlug1}/{slug}")]
    [Route("/directories/results/{parentSlug2}/{parentSlug1}/{slug}")]
    [Route("/directories/results/{parentSlug3}/{parentSlug2}/{parentSlug1}/{slug}")]
    [Route("/directories/results/{parentSlug4}/{parentSlug3}/{parentSlug2}/{parentSlug1}/{slug}")]
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
        
        
        //directoryViewModel.Directory.AddToRouteValuesIfNotNullOrEmpty(processedDirectory.Slug, true);
        //directoryViewModel.Directory.AddToRouteValuesIfNotNullOrEmpty(parentSlug1);
        //directoryViewModel.Directory.AddToRouteValuesIfNotNullOrEmpty(parentSlug2);
        //directoryViewModel.Directory.AddToRouteValuesIfNotNullOrEmpty(parentSlug3);
        //directoryViewModel.Directory.AddToRouteValuesIfNotNullOrEmpty(parentSlug4);

        //if (!string.IsNullOrEmpty(parentSlug1))
        //{
        //    var directoryLookupHttpResponse = await _directoryRepository.Get<Directory>(parentSlug1);

        //    if (directoryLookupHttpResponse.IsSuccessful())
        //    {
        //        var lookedUpDirectory = directoryLookupHttpResponse.Content as Directory;
        //        directoryViewModel.Breadcrumbs.Add(new Crumb(lookedUpDirectory.Title, parentSlug1, "Directories"));
        //    }
        //}
        
        
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

        var processedEntryDirectory = directoryEntryHttpResponse.Content as DirectoryEntry;

        // Anything before the (last) entry slug is directory hierarchy
        var directorySlugs = slugValues.SkipLast(1).ToList();
        List<Directory> parentDirectories = new();

        // Get the parent directories
        HttpResponse directoryHttpResponse;
        foreach (var directorySlug in directorySlugs)
        {
            directoryHttpResponse = await _directoryRepository.Get<Directory>(directorySlug);
            if (!directoryHttpResponse.IsSuccessful())
                return directoryEntryHttpResponse;

            var p = directoryHttpResponse.Content as Directory;
            parentDirectories.Add(p);
        }

        // Create breadcrumbs for the parent
        List<Crumb> breadcrumbs = new();
        for (int i = 0; i < parentDirectories.Count; i++)
        {
            var directory = parentDirectories[i];
            var relativeParentDirectories = parentDirectories.GetRange(0, i);
            var directorySlug = string.Join('/', relativeParentDirectories);
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