using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.ContentFactory;

public class ContentTypeFactory
{
    private readonly Dictionary<Type, dynamic> _factories = new Dictionary<Type, object>();

    public ContentTypeFactory(
        ITagParserContainer tagParserContainer,
        IDynamicTagParser<Profile> profileTagParser,
        MarkdownWrapper markdownWrapper,
        IDynamicTagParser<Document> documentTagParser,
        IDynamicTagParser<Alert> alertsInlineTagParser,
        IHttpContextAccessor httpContextAccesor,
        IDynamicTagParser<PrivacyNotice> privacyNoticeTagParser)
    {
        var sectionFactory = new SectionFactory(tagParserContainer, profileTagParser, markdownWrapper, documentTagParser, alertsInlineTagParser, privacyNoticeTagParser, null);
        var contactUsCategoryFactory = new ContactUsCategoryFactory(tagParserContainer, markdownWrapper, documentTagParser, null);
        var triviaFactory = new TriviaFactory(markdownWrapper);

        _factories.Add(typeof(Section), sectionFactory);
        _factories.Add(typeof(Article), new ArticleFactory(tagParserContainer, sectionFactory, markdownWrapper, null));
        _factories.Add(typeof(DocumentPage), new DocumentPageFactory(markdownWrapper));
        _factories.Add(typeof(News), new NewsFactory(tagParserContainer, markdownWrapper, documentTagParser, profileTagParser));
        _factories.Add(typeof(Event), new EventFactory(tagParserContainer, markdownWrapper, documentTagParser));
        _factories.Add(typeof(Homepage), new HomepageFactory(markdownWrapper));
        _factories.Add(typeof(GroupHomepage), new GroupHomepageFactory(tagParserContainer, markdownWrapper));
        _factories.Add(typeof(Group), new GroupFactory(tagParserContainer, markdownWrapper));
        _factories.Add(typeof(Payment), new PaymentFactory(tagParserContainer, markdownWrapper));
        _factories.Add(typeof(ServicePayPayment), new ServicePayPaymentFactory(tagParserContainer, markdownWrapper));
        _factories.Add(typeof(Showcase), new ShowcaseFactory(tagParserContainer, markdownWrapper, triviaFactory));
        _factories.Add(typeof(Organisation), new OrganisationFactory(markdownWrapper, httpContextAccesor));
        _factories.Add(typeof(PrivacyNotice), new PrivacyNoticeFactory(markdownWrapper));
        _factories.Add(typeof(StartPage), new StartPageFactory(tagParserContainer, markdownWrapper, alertsInlineTagParser));
        _factories.Add(typeof(ContactUsArea), new ContactUsAreaFactory(tagParserContainer, markdownWrapper, contactUsCategoryFactory));
        _factories.Add(typeof(List<Trivia>), triviaFactory);
    }

    public IProcessedContentType Build<T>(T content)
    {
        return _factories[content.GetType()].Build(content);
    }
}