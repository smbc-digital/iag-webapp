namespace StockportWebapp.Controllers;

public class DocumentController : Controller
{
    private readonly IProcessedContentRepository _repository;
    private readonly IContactUsMessageTagParser _contactUsMessageParser;

    public DocumentController(
        IProcessedContentRepository repository,
        IContactUsMessageTagParser contactUsMessageParser)
    {
        _repository = repository;
        _contactUsMessageParser = contactUsMessageParser;
    }

    [Route("/documents/{documentPageSlug}")]
    public async Task<IActionResult> Index(string documentPageSlug)
    {
        HttpResponse documentPageHttpResponse = await _repository.Get<DocumentPage>(documentPageSlug);

        if (!documentPageHttpResponse.IsSuccessful())
            return documentPageHttpResponse;

        DocumentPage documentPage = documentPageHttpResponse.Content as DocumentPage;

        DocumentPageViewModel viewModel = new(documentPage);

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();

        return View("Index2024", viewModel);
    }
}