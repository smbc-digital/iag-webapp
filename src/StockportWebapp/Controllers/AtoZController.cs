namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Long)]
public class AtoZController(IRepository repository, IFeatureManager featureManager) : Controller
{
    private readonly IRepository _repository = repository;
    private readonly IFeatureManager _featureManager = featureManager;

    [Route("/atoz/")]
    public async Task<IActionResult> Index()
    {
        HttpResponse httpResponse = await _repository.Get<List<AtoZ>>();

        if (httpResponse is null)
            return StatusCode(500, "Internal server error");

        if (httpResponse.IsNotAuthorised())
            return StatusCode(401, "Unauthorized");

        List<AtoZ> response = new();

        if (!httpResponse.IsSuccessful())
            ViewBag.Error = httpResponse.Content;
        else
            response = httpResponse.Content as List<AtoZ>;

        if (await _featureManager.IsEnabledAsync("AtoZPage"))
        {
            return View("Index2025", new AtoZViewModel()
            {
                Items = response,
                CurrentLetter = "A to Z",
                Breadcrumbs = new List<Crumb>()
            });
        }

        return View(new AtoZViewModel()
        {
            Items = response,
            Breadcrumbs = new List<Crumb>()
        });
    }

    [Route("/atoz/{letter}")]
    public async Task<IActionResult> Index(string letter)
    {
        if (IsNotInTheAlphabet(letter)) return NotFound();

        HttpResponse httpResponse = await _repository.Get<List<AtoZ>>(letter);
        
        if (httpResponse.IsNotAuthorised())
            return new HttpResponse(500, string.Empty, "Error");

        List<AtoZ> response = new();
    
        if (!httpResponse.IsSuccessful())
            ViewBag.Error = httpResponse.Content;
        else
            response = httpResponse.Content as List<AtoZ>;

        AtoZViewModel model = new()
        {
            Items = response,
            CurrentLetter = letter.ToUpper(),
            Breadcrumbs = new List<Crumb>()
        };
    
        if (await _featureManager.IsEnabledAsync("AtoZPage"))
            return View("Index2025", model);

        return View(model);
    }

    private static bool IsNotInTheAlphabet(string letter) =>
        string.IsNullOrEmpty(letter) || !letter.Length.Equals(1) || !char.IsLetter(letter[0]);
}