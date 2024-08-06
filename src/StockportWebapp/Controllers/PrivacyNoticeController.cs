namespace StockportWebapp.Controllers;

public class PrivacyNoticeController : Controller
{
    private readonly IProcessedContentRepository _repository;
    private readonly IFeatureManager _featureManager;

    public PrivacyNoticeController(IProcessedContentRepository repository, IFeatureManager featureManager = null)
    {
        _repository = repository;
        _featureManager = featureManager;
    } 

    [HttpGet]
    [Route("/privacy-notices/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        HttpResponse result = await _repository.Get<PrivacyNotice>(slug);

        if (!result.IsSuccessful())
            return result;

        PrivacyNoticeViewModel viewModel = new(result.Content as ProcessedPrivacyNotice);

        if (_featureManager is not null && _featureManager.IsEnabledAsync("PrivacyNotices").Result)
            return View("Detail2024", viewModel);
        
        return View(viewModel);
    }
}