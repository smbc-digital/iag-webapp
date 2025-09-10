namespace StockportWebapp.ContentFactory;

public class NewsFactory(ITagParserContainer simpleTagParserContainer,
                        MarkdownWrapper markdownWrapper)
{
    private readonly ITagParserContainer _tagParserContainer = simpleTagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

    public virtual ProcessedNews Build(News news)
    {
        string body = _markdownWrapper.ConvertToHtml(news.Body ?? string.Empty);

        body = _tagParserContainer.ParseAll(body, news.Title, true, null, null, news.InlineQuotes, null, null, null, true);

        return new ProcessedNews(news.Title,
                                news.Slug,
                                news.Teaser,
                                news.HeroImage,
                                news.Image,
                                news.ThumbnailImage,
                                news.HeroImageCaption,
                                body,
                                news.SunriseDate,
                                news.PublishingDate,
                                news.SunsetDate,
                                news.UpdatedAt,
                                news.Alerts,
                                news.Tags,
                                news.InlineQuotes,
                                news.CallToAction,
                                news.LogoAreaTitle,
                                news.TrustedLogos,
                                news.FeaturedLogo,
                                news.EventsByTagOrCategory,
                                news.Events);
    }
}