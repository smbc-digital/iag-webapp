namespace StockportWebapp.Controllers;

public class DocumentController : Controller
{
    private readonly IProcessedContentRepository _repository;
    private readonly IDocumentPageRepository _documentPageRepository;
    private readonly IContactUsMessageTagParser _contactUsMessageParser;

    public DocumentController(
        IProcessedContentRepository repository,
        IContactUsMessageTagParser contactUsMessageParser,
        IDocumentPageRepository documentPageRepository
        )
    {
        _repository = repository;
        _contactUsMessageParser = contactUsMessageParser;
        _documentPageRepository = documentPageRepository;
    }

    [Route("/documents/{documentPageSlug}")]
    public async Task<IActionResult> Index(string documentPageSlug)
    {
        var documentPageHttpResponse = await _documentPageRepository.Get(documentPageSlug);

        if (!documentPageHttpResponse.IsSuccessful())
            return documentPageHttpResponse;

        var documentPage = documentPageHttpResponse.Content as ProcessedDocumentPage;

        var viewModel = new DocumentPageViewModel(documentPage);

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();

        return View(viewModel);
    }
}