namespace StockportWebapp.ViewComponents;

public class SemanticFooterViewComponent(IRepository repository,
                                        ILogger<SemanticFooterViewComponent> logger) : ViewComponent
{
    private readonly IRepository _repository = repository;
    private readonly ILogger<SemanticFooterViewComponent> _logger = logger;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        _logger.LogInformation("Call to retrieve the footer");

        HttpResponse footerHttpResponse = await _repository.Get<Footer>();

        if (!footerHttpResponse.IsSuccessful())
            return await Task.FromResult(View("NoFooterFound"));

        Footer model = footerHttpResponse.Content as Footer;

        return await Task.FromResult(View("Semantic", model));
    }
}