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

        if (profileEntity != null)
        {
            var model = new Models.Profile(profileEntity.Title,
                profileEntity.Slug,
                profileEntity.Subtitle,
                profileEntity.Quote,
                profileEntity.Image,
                profileEntity.Body,
                profileEntity.Breadcrumbs,
                profileEntity.Alerts,
                profileEntity.TriviaSubheading,
                profileEntity.TriviaSection,
                profileEntity.InlineQuotes,
                profileEntity.EventsBanner);

            return View(model);
        }

        return NotFound();
    }
}
