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

    [Route("/directory/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        var directoryHttpResponse = await _directoryRepository.Get<Directory>(slug);
        if (!directoryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        var processedDirectory = directoryHttpResponse.Content as ProcessedDirectory;

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = processedDirectory
        };

        return View("Directory", directoryViewModel);
    }
}