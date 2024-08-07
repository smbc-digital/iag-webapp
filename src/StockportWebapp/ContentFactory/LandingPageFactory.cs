namespace StockportWebapp.ContentFactory;

public class LandingPageFactory
{
    private readonly ITagParserContainer _tagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly ITriviaFactory _triviaFactory;

    public LandingPageFactory(ITagParserContainer tagParserContainer,
        MarkdownWrapper markdownWrapper,
        ITriviaFactory triviaFactory)
    {
        _tagParserContainer = tagParserContainer;
        _markdownWrapper = markdownWrapper;
        _triviaFactory = triviaFactory;
    }

    public virtual LandingPage Build(LandingPage landingPage)
    {
        throw new NotImplementedException();
    }
}
