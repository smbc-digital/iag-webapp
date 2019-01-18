using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using StockportWebapp.ContentFactory.InformationFactory;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.ContentFactory
{

    public class ContentTypeFactory
    {
        private readonly Dictionary<Type, dynamic> _factories = new Dictionary<Type, object>();

        public ContentTypeFactory(ISimpleTagParserContainer tagParserContainer, IDynamicTagParser<Profile> profileTagParser, MarkdownWrapper markdownWrapper, IDynamicTagParser<Document> documentTagParser, IDynamicTagParser<Alert> alertsInlineTagParser, IHttpContextAccessor httpContextAccesor, IDynamicTagParser<S3BucketSearch> s3BucketParser, IDynamicTagParser<PrivacyNotice> privacyNoticeTagParser, IInformationFactory informationFactory)
        {
            var sectionFactory = new SectionFactory(tagParserContainer, profileTagParser, markdownWrapper, documentTagParser, alertsInlineTagParser, s3BucketParser, privacyNoticeTagParser, null);
            var contactUsCategoryFactory = new ContactUsCategoryFactory(tagParserContainer, markdownWrapper,documentTagParser, null);

            _factories.Add(typeof(Section), sectionFactory);
            _factories.Add(typeof(Profile), new ProfileFactory(tagParserContainer, markdownWrapper, alertsInlineTagParser));
            _factories.Add(typeof(Article), new ArticleFactory(tagParserContainer, profileTagParser, sectionFactory, markdownWrapper, documentTagParser, alertsInlineTagParser, s3BucketParser, privacyNoticeTagParser, null));
            _factories.Add(typeof(News), new NewsFactory(tagParserContainer, markdownWrapper, documentTagParser));
            _factories.Add(typeof(Event), new EventFactory(tagParserContainer, markdownWrapper, documentTagParser));
            _factories.Add(typeof(Homepage), new HomepageFactory(markdownWrapper));
            _factories.Add(typeof(GroupHomepage), new GroupHomepageFactory(tagParserContainer, markdownWrapper));
            _factories.Add(typeof(Group), new GroupFactory(tagParserContainer, markdownWrapper, documentTagParser));
            _factories.Add(typeof(Payment), new PaymentFactory(tagParserContainer, markdownWrapper));
            _factories.Add(typeof(Showcase), new ShowcaseFactory(tagParserContainer, markdownWrapper));
            _factories.Add(typeof(Organisation), new OrganisationFactory(markdownWrapper, httpContextAccesor));
            _factories.Add(typeof(PrivacyNotice), new PrivacyNoticeFactory(markdownWrapper));
            _factories.Add(typeof(StartPage), new StartPageFactory(tagParserContainer, markdownWrapper, alertsInlineTagParser));
            _factories.Add(typeof(ContactUsArea), new ContactUsAreaFactory(tagParserContainer, markdownWrapper, contactUsCategoryFactory));
            _factories.Add(typeof(List<InformationItem>), new InformationFactory.InformationFactory(markdownWrapper));
        }

        public IProcessedContentType Build<T>(T content)
        {
            return _factories[content.GetType()].Build(content);
        }
    }
}