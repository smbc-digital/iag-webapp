namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class ContactUsAreaController(IProcessedContentRepository repository) : Controller
{
    private readonly IProcessedContentRepository _repository = repository;

    [Route("/contact")]
    public async Task<IActionResult> Index()
    {
        HttpResponse contactUsAreaHttpResponse = await _repository.Get<ContactUsArea>();

        if (!contactUsAreaHttpResponse.IsSuccessful())
            return contactUsAreaHttpResponse;

        return View(contactUsAreaHttpResponse.Content as ProcessedContactUsArea);
    }
}