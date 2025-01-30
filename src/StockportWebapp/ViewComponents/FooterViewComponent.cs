namespace StockportWebapp.ViewComponents;

public class FooterViewComponent(IRepository repository,
                                ILogger<FooterViewComponent> logger, MarkdownWrapper markdownWrapper) : ViewComponent
{
    private readonly IRepository _repository = repository;
    private readonly ILogger<FooterViewComponent> _logger = logger;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        _logger.LogInformation("Call to retrieve the footer");

        HttpResponse footerHttpResponse = await _repository.Get<Footer>();

        if (!footerHttpResponse.IsSuccessful())
            return await Task.FromResult(View("NoFooterFound"));

        Footer model = footerHttpResponse.Content as Footer;
        model.FooterContent1 = _markdownWrapper.ConvertToHtml(model.FooterContent1);
        model.FooterContent2 = _markdownWrapper.ConvertToHtml(model.FooterContent2);
        model.FooterContent3 = _markdownWrapper.ConvertToHtml(model.FooterContent3);
        
        return await Task.FromResult(View(model));
    }
}