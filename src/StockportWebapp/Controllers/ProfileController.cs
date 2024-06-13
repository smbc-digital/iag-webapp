namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Long)]
public class ProfileController : Controller
{
    private readonly IProcessedContentRepository _repository;
    private readonly IProfileService _profileService;

    public ProfileController(IProcessedContentRepository repository, IProfileService profileService)
    {
        _repository = repository;
        _profileService = profileService;
    }

    [Route("/profile/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        var profileEntity = await _profileService.GetProfile(slug);

        if (profileEntity is not null)
        {
            ProfileViewModel viewModel = new(profileEntity);
            return View(viewModel);
        }

        return NotFound();
    }
}