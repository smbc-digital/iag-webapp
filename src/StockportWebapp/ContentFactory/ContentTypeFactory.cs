using System;
using System.Collections.Generic;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{

    public class ContentTypeFactory
    {
        private readonly Dictionary<Type, dynamic> _factories = new Dictionary<Type, object>();

        public ContentTypeFactory(ISimpleTagParserContainer tagParserContainer, IDynamicTagParser<Profile> profileTagParser, MarkdownWrapper markdownWrapper, IDynamicTagParser<Document> documentTagParser, IDynamicTagParser<Alert> alertsInlineTagParser)
        {
            var sectionFactory = new SectionFactory(tagParserContainer, profileTagParser, markdownWrapper, documentTagParser, alertsInlineTagParser);

            _factories.Add(typeof(Section), sectionFactory);
            _factories.Add(typeof(Profile), new ProfileFactory(tagParserContainer, markdownWrapper));
            _factories.Add(typeof(Article), new ArticleFactory(tagParserContainer, profileTagParser, sectionFactory, markdownWrapper, documentTagParser, alertsInlineTagParser));
            _factories.Add(typeof(News), new NewsFactory(tagParserContainer, markdownWrapper, documentTagParser));
            _factories.Add(typeof(Event), new EventFactory(tagParserContainer, markdownWrapper, documentTagParser));
            _factories.Add(typeof(Homepage), new HomepageFactory(markdownWrapper));
            _factories.Add(typeof(Group), new GroupFactory(tagParserContainer, markdownWrapper));
            _factories.Add(typeof(Payment), new PaymentFactory(tagParserContainer, markdownWrapper));
            _factories.Add(typeof(Showcase), new ShowcaseFactory());
        }

        public IProcessedContentType Build<T>(T content)
        {
            return _factories[content.GetType()].Build(content);
        }
    }
}