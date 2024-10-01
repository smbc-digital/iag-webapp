namespace StockportWebapp.Controllers;

public class DocumentController : Controller
{
    private readonly IDocumentPageRepository _documentPageRepository;
    private readonly IContactUsMessageTagParser _contactUsMessageParser;

    public DocumentController(
        IDocumentPageRepository documentPageRepository,
        IContactUsMessageTagParser contactUsMessageParser)
    {
        _documentPageRepository = documentPageRepository;
        _contactUsMessageParser = contactUsMessageParser;
    }

    [Route("/documents/{documentPageSlug}")]
    public async Task<IActionResult> Index(string documentPageSlug)
    {
        HttpResponse result = await _documentPageRepository.Get(documentPageSlug);

        if (!result.IsSuccessful())
            return result;

        DocumentPageViewModel viewModel = new(result.Content as DocumentPage);

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();

        return View("Index2024", viewModel);
    }
}