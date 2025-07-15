namespace StockportWebapp.ContentFactory;

public class NewsFactory(ITagParserContainer simpleTagParserContainer,
                        MarkdownWrapper markdownWrapper)
{
    private readonly ITagParserContainer _tagParserContainer = simpleTagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

    public virtual ProcessedNews Build(News news)
    {
        string body = _markdownWrapper.ConvertToHtml(news.Body ?? string.Empty);

        body = _tagParserContainer.ParseAll(body, news.Title, true, null, news.Documents, news.InlineQuotes, null, news.Profiles, null, true);

        return new ProcessedNews(news.Title,
                                news.Slug,
                                news.Teaser,
                                news.Purpose,
                                news.HeroImage,
                                news.Image,
                                news.ThumbnailImage,
                                news.HeroImageCaption,
                                body,
                                news.Breadcrumbs,
                                news.SunriseDate,
                                news.SunsetDate,
                                news.SunriseDate2,
                                news.SunsetDate2,
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