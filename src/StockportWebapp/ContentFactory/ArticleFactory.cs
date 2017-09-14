using System.Collections.Generic;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class ArticleFactory
    {
        private readonly ISimpleTagParserContainer _tagParserContainer;
        private readonly IDynamicTagParser<Profile> _profileTagParser;
        private readonly IDynamicTagParser<Alert> _alertsInlineTagParser;
        private readonly ISectionFactory _sectionFactory;
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly IDynamicTagParser<Document> _documentTagParser;

        public ArticleFactory(ISimpleTagParserContainer tagParserContainer, IDynamicTagParser<Profile> profileTagParser, ISectionFactory sectionFactory, MarkdownWrapper markdownWrapper, IDynamicTagParser<Document> documentTagParser, IDynamicTagParser<Alert> alertsInlineTagParser)
        {
            _tagParserContainer = tagParserContainer;
            _sectionFactory = sectionFactory;
            _markdownWrapper = markdownWrapper;
            _profileTagParser = profileTagParser;
            _documentTagParser = documentTagParser;
            _alertsInlineTagParser = alertsInlineTagParser;
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
            if (article.LiveChat != null)
            {
                article.LiveChat.Text = _markdownWrapper.ConvertToHtml(article.LiveChat.Text ?? "");
            }
            body = _profileTagParser.Parse(body, article.Profiles);
            body = _documentTagParser.Parse(body, article.Documents);
            body = _alertsInlineTagParser.Parse(body, article.AlertsInline);

            return new ProcessedArticle(article.Title, article.Slug, body, article.Teaser, 
                processedSections, article.Icon, article.BackgroundImage, article.Image, article.Breadcrumbs, article.Alerts, article.ParentTopic, article.LiveChatVisible, article.LiveChat, article.AlertsInline, article.Advertisement);
        }
    }
}
