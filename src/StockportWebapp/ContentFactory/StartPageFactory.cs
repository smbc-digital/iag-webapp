namespace StockportWebapp.ContentFactory;

public class StartPageFactory(ITagParserContainer tagParserContainer,
                            MarkdownWrapper markdownWrapper)
{
    private readonly ITagParserContainer _tagParserContainer = tagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

    public virtual ProcessedStartPage Build(StartPage startPage)
    {
        string upperBody = _tagParserContainer.ParseAll(startPage.UpperBody ?? string.Empty, startPage.Title, true, startPage.AlertsInline, null, null, null, null);
        string lowerBody = _tagParserContainer.ParseAll(startPage.LowerBody ?? string.Empty, startPage.Title, true, startPage.AlertsInline, null, null, null, null);

        return new ProcessedStartPage(
            startPage.Slug,
            startPage.Title,
            startPage.Teaser,
            startPage.Summary,
            _markdownWrapper.ConvertToHtml(upperBody ?? string.Empty),
            startPage.FormLink,
            _markdownWrapper.ConvertToHtml(lowerBody ?? string.Empty),
            startPage.Breadcrumbs,
            startPage.BackgroundImage,
            startPage.Icon,
            startPage.Alerts
        );
    }
}