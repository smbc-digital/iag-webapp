namespace StockportWebapp.Controllers;

public class PrivacyNoticeController : Controller
{
    private readonly IProcessedContentRepository _repository;

    public PrivacyNoticeController(IProcessedContentRepository repository) => _repository = repository;

    [HttpGet]
    [Route("/privacy-notices/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        HttpResponse result = await _repository.Get<PrivacyNotice>(slug);

        if (!result.IsSuccessful())
            return result;

        return View("Index", new PrivacyNoticeViewModel(result.Content as ProcessedPrivacyNotice));
    }
}