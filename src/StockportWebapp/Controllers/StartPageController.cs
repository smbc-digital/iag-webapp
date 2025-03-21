﻿namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class StartPageController(IProcessedContentRepository processedContnentRepository) : Controller
{
    private readonly IProcessedContentRepository _processedContentRepository = processedContnentRepository;

    [HttpGet]
    [Route("/start/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        HttpResponse response = await _processedContentRepository.Get<StartPage>(slug);

        if (!response.IsSuccessful()) 
            return response;

        ProcessedStartPage startPage = response.Content as ProcessedStartPage;
        
        return View(startPage);
    }
}