namespace StockportWebapp.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    private readonly IRepository _repository;
    private readonly ILogger<FooterViewComponent> _logger;
    private readonly MarkdownWrapper _markdownWrapper;

    public FooterViewComponent(IRepository repository, ILogger<FooterViewComponent> logger, MarkdownWrapper markdownWrapper)
    {
        _repository = repository;
        _logger = logger;
        _markdownWrapper = markdownWrapper;
    }

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