namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Long)]
public class ProfileController : Controller
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService) => _profileService = profileService;

    [Route("/profile/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        Models.Profile profile = await _profileService.GetProfile(slug);

        if (profile is null)
            return NotFound();

        return View(new ProfileViewModel(profile));
    }
}