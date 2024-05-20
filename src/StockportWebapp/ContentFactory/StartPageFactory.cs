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
        var upperBody = _tagParserContainer.ParseAll(startPage.UpperBody ?? "", startPage.Title, true, startPage.AlertsInline, null, null, null, null);

        upperBody = _markdownWrapper.ConvertToHtml(upperBody ?? "");

        return new ProcessedStartPage(
        startPage.Slug,
        startPage.Title,
        startPage.Teaser,
        startPage.Summary,
        upperBody,
        startPage.FormLinkLabel,
        startPage.FormLink,
        startPage.LowerBody,
        startPage.Breadcrumbs,
        startPage.BackgroundImage,
        startPage.Icon,
        startPage.Alerts
        );


    }
}
