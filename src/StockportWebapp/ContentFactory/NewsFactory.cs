namespace StockportWebapp.ContentFactory;

public class NewsFactory
{
    private readonly ITagParserContainer _tagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper;

    public NewsFactory(ITagParserContainer simpleTagParserContainer, MarkdownWrapper markdownWrapper)
    {
        _tagParserContainer = simpleTagParserContainer;
        _markdownWrapper = markdownWrapper;
    }

    public virtual ProcessedNews Build(News news)
    {
        string body = _markdownWrapper.ConvertToHtml(news.Body ?? "");

        body = _tagParserContainer.ParseAll(body, news.Title, true, null, news.Documents, null, null, news.Profiles);

        return new ProcessedNews(news.Title, news.Slug, news.Teaser, news.Purpose, news.Image, news.ThumbnailImage, body, news.Breadcrumbs, news.SunriseDate, news.SunsetDate, news.UpdatedAt, news.Alerts, news.Tags);
    }
}