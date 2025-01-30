namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
[Obsolete("Groups is being replaced by directories/directory entries")]
[ExcludeFromCodeCoverage(Justification = "Obsolete")]
public class OrganisationsController(IProcessedContentRepository repository) : Controller
{
    private readonly IProcessedContentRepository _repository = repository;

    [ResponseCache(NoStore = true, Duration = 0)]
    [Route("/organisations/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        HttpResponse response = await _repository.Get<Organisation>(slug);

        if (!response.IsSuccessful())
            return response;

        ProcessedOrganisation organisation = response.Content as ProcessedOrganisation;

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();

        return View(organisation);
    }
}