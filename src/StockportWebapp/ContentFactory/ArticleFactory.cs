using System.Collections.Generic;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class ArticleFactory
    {
        private readonly ISimpleTagParserContainer _tagParserContainer;
        private readonly IDynamicTagParser<Profile> _profileTagParser;
        private readonly ISectionFactory _sectionFactory;
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly IDynamicTagParser<Document> _documentTagParser;

        public ArticleFactory(ISimpleTagParserContainer tagParserContainer, IDynamicTagParser<Profile> profileTagParser, ISectionFactory sectionFactory, MarkdownWrapper markdownWrapper, IDynamicTagParser<Document> documentTagParser)
        {
            _tagParserContainer = tagParserContainer;
            _sectionFactory = sectionFactory;
            _markdownWrapper = markdownWrapper;
            _profileTagParser = profileTagParser;
            _documentTagParser = documentTagParser;
        }

        public virtual ProcessedArticle Build(Article article)
        {
            var processedSections = new List<ProcessedSection>();
            foreach (var section in article.Sections)
            {
                processedSections.Add(_sectionFactory.Build(section, article.Title));
            }

            var body = _tagParserContainer.ParseAll(article.Body, article.Title);
            body = _markdownWrapper.ConvertToHtml(body ?? "");
            body = _profileTagParser.Parse(body, article.Profiles);
            body = _documentTagParser.Parse(body, article.Documents);

            return new ProcessedArticle(article.Title, article.Slug, body, article.Teaser, 
                processedSections, article.Icon, article.BackgroundImage, article.Breadcrumbs, article.Alerts, article.ParentTopic);
        }
    }
}
