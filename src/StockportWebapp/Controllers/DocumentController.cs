namespace StockportWebapp.Controllers;

public class DocumentController : Controller
{
    private readonly IDocumentPageRepository _documentPageRepository;
    private readonly IContactUsMessageTagParser _contactUsMessageParser;
    private readonly IFeatureManager _featureManager;

    public DocumentController(
        IDocumentPageRepository documentPageRepository,
        IContactUsMessageTagParser contactUsMessageParser,
        IFeatureManager featureManager = null)
    {
        _documentPageRepository = documentPageRepository;
        _contactUsMessageParser = contactUsMessageParser;
        _featureManager = featureManager;
    }

    [Route("/documents/{documentPageSlug}")]
    public async Task<IActionResult> Index(string documentPageSlug)
    {
        HttpResponse result = await _documentPageRepository.Get(documentPageSlug);

        if (!result.IsSuccessful())
            return result;

        DocumentPageViewModel viewModel = new(result.Content as DocumentPage);

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();

        if (_featureManager is not null && _featureManager.IsEnabledAsync("DocumentPages").Result)
            return View("Index2024", viewModel);
        
        return View(viewModel);
    }
}