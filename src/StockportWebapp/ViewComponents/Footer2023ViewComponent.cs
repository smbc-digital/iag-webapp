namespace StockportWebapp.ViewComponents;

[ExcludeFromCodeCoverage]
public class Footer2023ViewComponent(IRepository repository, ILogger<FooterViewComponent> logger) : ViewComponent
{
    private readonly IRepository _repository = repository;
    private readonly ILogger<FooterViewComponent> _logger = logger;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        _logger.LogInformation("Call to retrieve the footer");

        HttpResponse footerHttpResponse = await _repository.Get<Footer>();

        if (!footerHttpResponse.IsSuccessful())
            return await Task.FromResult(View("NoFooterFound"));

        Footer model = footerHttpResponse.Content as Footer;

        return await Task.FromResult(View(model));
    }
}