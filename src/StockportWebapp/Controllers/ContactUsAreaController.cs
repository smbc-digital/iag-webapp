namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class ContactUsAreaController : Controller
{
    private readonly IProcessedContentRepository _repository;

    public ContactUsAreaController(IProcessedContentRepository repository)
        => _repository = repository;

    [Route("/contact")]
    public async Task<IActionResult> Index()
    {
        var contactUsAreaHttpResponse = await _repository.Get<ContactUsArea>();
        if (!contactUsAreaHttpResponse.IsSuccessful())
            return contactUsAreaHttpResponse;

        var contactUsArea = contactUsAreaHttpResponse.Content as ProcessedContactUsArea;

        return View(contactUsArea);
    }
}