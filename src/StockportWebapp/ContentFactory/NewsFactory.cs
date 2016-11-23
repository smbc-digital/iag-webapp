using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class NewsFactory
    {
        private readonly ISimpleTagParserContainer _simpleTagParserContainer;
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly IDynamicTagParser<Document> _documentTagParser;

        public NewsFactory(ISimpleTagParserContainer simpleTagParserContainer, MarkdownWrapper markdownWrapper, IDynamicTagParser<Document> documentTagParser)
        {
            _simpleTagParserContainer = simpleTagParserContainer;
            _markdownWrapper = markdownWrapper;
            _documentTagParser = documentTagParser;
        }

        public virtual ProcessedNews Build(News news)
        {
            var body = _simpleTagParserContainer.ParseAll(news.Body,news.Title);
            body = _markdownWrapper.ConvertToHtml(body ?? "");
            body = _documentTagParser.Parse(body, news.Documents);

            return new ProcessedNews(news.Title, news.Slug, news.Teaser, news.Image, news.ThumbnailImage, body, news.Breadcrumbs, news.SunriseDate, news.SunsetDate, news.Alerts, news.Tags);
        }
    }
}
