﻿namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Long)]
public class AtoZController(IRepository repository) : Controller
{
    private readonly IRepository _repository = repository;

    [Route("/atoz/")]
    public async Task<IActionResult> Index()
    {
        HttpResponse httpResponse = await _repository.Get<List<AtoZ>>();

        if (httpResponse is null)
            return StatusCode(500, "Internal server error");

        List<AtoZ> response = new();

        if (!httpResponse.IsSuccessful())
            ViewBag.Error = httpResponse.Content;
        else
            response = httpResponse.Content as List<AtoZ>;

        return View(new AtoZViewModel()
        {
            Items = response,
            CurrentLetter = "A to Z",
            Breadcrumbs = new List<Crumb>()
        });
    }

    [Route("/atoz/{letter}")]
    public async Task<IActionResult> Index(string letter)
    {
        if (IsNotInTheAlphabet(letter)) return NotFound();

        HttpResponse httpResponse = await _repository.Get<List<AtoZ>>(letter);
        
        if (httpResponse is null)
            return StatusCode(500, "Internal server error");

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
    
        return View(model);
    }

    private static bool IsNotInTheAlphabet(string letter) =>
        string.IsNullOrEmpty(letter) || !letter.Length.Equals(1) || !char.IsLetter(letter[0]);
}