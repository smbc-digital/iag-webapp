namespace StockportWebapp.ViewComponents;

public class HeaderViewComponent(IRepository repository, ILogger<HeaderViewComponent> logger) : ViewComponent
{
    private readonly IRepository _repository = repository;
    private readonly ILogger<HeaderViewComponent> _logger = logger;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        _logger.LogInformation("Call to retrieve the header");

        HttpResponse httpResponse = await _repository.Get<SiteHeader>();

        if (!httpResponse.IsSuccessful())
           throw new FileNotFoundException();

        SiteHeader model = httpResponse.Content as SiteHeader;

        return await Task.FromResult(View(model));
    }
}