namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class ContactUsAreaController(IProcessedContentRepository repository, IFeatureManager featureManager) : Controller
{
    private readonly IProcessedContentRepository _repository = repository;
    private readonly IFeatureManager _featureManager = featureManager;

    [Route("/contact")]
    public async Task<IActionResult> Index()
    {
        HttpResponse contactUsAreaHttpResponse = await _repository.Get<ContactUsArea>();

        if (!contactUsAreaHttpResponse.IsSuccessful())
            return contactUsAreaHttpResponse;

        if (await _featureManager.IsEnabledAsync("ContactUsArea"))
            return View("Index2025", contactUsAreaHttpResponse.Content as ProcessedContactUsArea);

        return View("Index", contactUsAreaHttpResponse.Content as ProcessedContactUsArea);
    }
}