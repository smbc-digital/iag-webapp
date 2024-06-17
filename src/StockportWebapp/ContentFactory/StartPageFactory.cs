namespace StockportWebapp.ContentFactory;

public class StartPageFactory
{
    private ITagParserContainer _tagParserContainer;
    private MarkdownWrapper _markdownWrapper;

    public StartPageFactory(ITagParserContainer tagParserContainer, MarkdownWrapper markdownWrapper)
    {
        _tagParserContainer = tagParserContainer;
        _markdownWrapper = markdownWrapper;
    }

    public virtual ProcessedStartPage Build(StartPage startPage)
    {
        string upperBody = _tagParserContainer.ParseAll(startPage.UpperBody ?? "", startPage.Title, true, startPage.AlertsInline, null, null, null, null);
        string lowerBody = _tagParserContainer.ParseAll(startPage.LowerBody ?? "", startPage.Title, true, startPage.AlertsInline, null, null, null, null);

        return new ProcessedStartPage(
            startPage.Slug,
            startPage.Title,
            startPage.Teaser,
            startPage.Summary,
            _markdownWrapper.ConvertToHtml(upperBody ?? ""),
            startPage.FormLinkLabel,
            startPage.FormLink,
            _markdownWrapper.ConvertToHtml(lowerBody ?? ""),
            startPage.Breadcrumbs,
            startPage.BackgroundImage,
            startPage.Icon,
            startPage.Alerts
        );
    }
}