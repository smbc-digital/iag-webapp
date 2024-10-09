namespace StockportWebapp.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    private readonly IRepository _repository;
    private readonly ILogger<HeaderViewComponent> _logger;

    public HeaderViewComponent(IRepository repository, ILogger<HeaderViewComponent> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        _logger.LogInformation("Call to retrieve the header");

        HttpResponse httpResponse = await _repository.Get<SiteHeader>();

        if (!httpResponse.IsSuccessful())
           throw new FileNotFoundException();

        SiteHeader model = httpResponse.Content as SiteHeader;

        return await Task.FromResult(View("Header", model));
    }
}