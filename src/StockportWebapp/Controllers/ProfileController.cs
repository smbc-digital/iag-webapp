namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Long)]
public class ProfileController : Controller
{
    private readonly IProfileService _profileService;
    private readonly BusinessId _businessId;
    private readonly IFeatureManager _featureManager;
    private readonly bool _isStockportGovProfile;
    private readonly bool _profilesToggle = true;

    public ProfileController(IProfileService profileService, BusinessId businessId, IFeatureManager featureManager = null)
    {
        _profileService = profileService;
        _businessId = businessId;
        _featureManager = featureManager;

        _isStockportGovProfile = _businessId.ToString().Equals("stockportgov");
        if (_featureManager is not null)
            _profilesToggle = _featureManager.IsEnabledAsync("Profiles").Result;
    }

    [Route("/profile/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        var profile = await _profileService.GetProfile(slug);

        if (profile is null)
            return NotFound();

        ProfileViewModel viewModel = new(profile);

        if (_isStockportGovProfile && _profilesToggle)
            return View("Index2024", viewModel);
        else
            return View(viewModel);
    }
}