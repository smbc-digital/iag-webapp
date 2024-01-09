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

    [Route("/directories/entry/{parentSlug1}/{slug}")]
    [Route("/directories/entry/{parentSlug2}/{parentSlug1}/{slug}")]
    [Route("/directories/entry/{parentSlug3}/{parentSlug2}/{parentSlug1}/{slug}")]
    [Route("/directories/entry/{parentSlug4}/{parentSlug3}/{parentSlug2}/{parentSlug1}/{slug}")]
    [Route("/directories/entry/{parentSlug5}/{parentSlug4}/{parentSlug3}/{parentSlug2}/{parentSlug1}/{slug}")]
    [Route("/directory-entry/{slug}")]
    public async Task<IActionResult> DirectoryEntry(string parentSlug, string slug)
    {
        var directoryHttpResponse = await _directoryRepository.Get<Directory>(parentSlug);
        var directoryEntryHttpResponse = await _directoryRepository.GetEntry<DirectoryEntry>(slug);
        if (!directoryHttpResponse.IsSuccessful() || !directoryEntryHttpResponse.IsSuccessful())
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