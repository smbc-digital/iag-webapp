namespace StockportWebapp.ViewComponents;

public class Footer2023ViewComponent : ViewComponent
{
    private readonly IRepository _repository;
    private readonly ILogger<FooterViewComponent> _logger;

    public Footer2023ViewComponent(IRepository repository, ILogger<FooterViewComponent> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        _logger.LogInformation("Call to retrieve the footer");

        var footerHttpResponse = await _repository.Get<Footer>();

        if (!footerHttpResponse.IsSuccessful())
            return await Task.FromResult(View("NoFooterFound"));

        var model = footerHttpResponse.Content as Footer;

        return await Task.FromResult(View(model));
    }
}