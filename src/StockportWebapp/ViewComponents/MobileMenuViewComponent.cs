namespace StockportWebapp.ViewComponents;

public class MobileMenuViewComponent(IHomepageService homepageService, ILogger<MobileMenuViewComponent> logger) : ViewComponent
{
    private readonly IHomepageService _homepageService = homepageService;
    private readonly ILogger<MobileMenuViewComponent> _logger = logger;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        _logger.LogInformation("Call to retrieve mobile menu content");

        ProcessedHomepage homepage = await _homepageService.GetHomepage();

        if (homepage is null)
            return await Task.FromResult(View("NoMobileMenuContent"));

        IEnumerable<SubItem> services = homepage.FeaturedTopics ?? Enumerable.Empty<SubItem>();

        return await Task.FromResult(View(services));
    }
}
