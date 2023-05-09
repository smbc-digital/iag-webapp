namespace StockportWebapp.ContentFactory;

public class StartPageFactory
{

    private ISimpleTagParserContainer _tagParserContainer;
    private IDynamicTagParser<Alert> _alertsInlineTagParser;
    private MarkdownWrapper _markdownWrapper;


    public StartPageFactory(ISimpleTagParserContainer tagParserContainer, MarkdownWrapper markdownWrapper, IDynamicTagParser<Alert> alertsInlineTagParser)
    {
        _tagParserContainer = tagParserContainer;
        _markdownWrapper = markdownWrapper;
        _alertsInlineTagParser = alertsInlineTagParser;
    }


    public virtual ProcessedStartPage Build(StartPage startPage)
    {

        var upperBody = _tagParserContainer.ParseAll(startPage.UpperBody ?? "", startPage.Title);

        upperBody = _markdownWrapper.ConvertToHtml(startPage.UpperBody ?? "");

        upperBody = _alertsInlineTagParser.Parse(upperBody, startPage.AlertsInline);



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
