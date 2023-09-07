﻿namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Long)]
public class AtoZController : Controller
{
    private readonly IRepository _repository;
    private readonly IFeatureManager _featureManager;

    public AtoZController(IRepository repository, IFeatureManager featureManager)
    {
        _repository = repository;
        _featureManager = featureManager;
    }

    [Route("/atoz/{letter}")]
    public async Task<IActionResult> Index(string letter)
    {
        if (IsNotInTheAlphabet(letter)) return NotFound();

        var httpResponse = await _repository.Get<List<AtoZ>>(letter);


        if (httpResponse.IsNotAuthorised())
            return new HttpResponse(500, "", "Error");

        var response = new List<AtoZ>();

        if (!httpResponse.IsSuccessful())
        {
            ViewBag.Error = httpResponse.Content;
        }
        else
        {
            response = httpResponse.Content as List<AtoZ>;
        }

        var model = new AtoZViewModel
        {
            Items = response,
            CurrentLetter = letter.ToUpper(),
            Breadcrumbs = new List<Crumb>()
        };

        if(await _featureManager.IsEnabledAsync("SiteRedesign"))
            return View("Index2023", model);

        return View(model);
    }

    private static bool IsNotInTheAlphabet(string letter)
    {
        return string.IsNullOrEmpty(letter) || letter.Length != 1 || !char.IsLetter(letter[0]);
    }
}
