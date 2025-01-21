namespace StockportWebapp.ContentFactory;

public class ContentTypeFactory
{
    private readonly Dictionary<Type, dynamic> _factories = new();

    public ContentTypeFactory(ITagParserContainer tagParserContainer,
                            MarkdownWrapper markdownWrapper,
                            IHttpContextAccessor httpContextAccessor,
                            IRepository repository)
    {
        SectionFactory sectionFactory = new(tagParserContainer, markdownWrapper, repository);
        ContactUsCategoryFactory contactUsCategoryFactory = new(tagParserContainer, markdownWrapper);
        TriviaFactory triviaFactory = new(markdownWrapper);

        _factories.Add(typeof(Section), sectionFactory);
        _factories.Add(typeof(Article), new ArticleFactory(tagParserContainer, sectionFactory, markdownWrapper, repository));
        _factories.Add(typeof(DocumentPage), new DocumentPageFactory(markdownWrapper));
        _factories.Add(typeof(News), new NewsFactory(tagParserContainer, markdownWrapper));
        _factories.Add(typeof(Event), new EventFactory(tagParserContainer, markdownWrapper));
        _factories.Add(typeof(Homepage), new HomepageFactory(markdownWrapper));
        _factories.Add(typeof(Payment), new PaymentFactory(tagParserContainer, markdownWrapper));
        _factories.Add(typeof(ServicePayPayment), new ServicePayPaymentFactory(tagParserContainer, markdownWrapper));
        _factories.Add(typeof(Showcase), new ShowcaseFactory(tagParserContainer, markdownWrapper, triviaFactory));
        _factories.Add(typeof(PrivacyNotice), new PrivacyNoticeFactory(markdownWrapper));
        _factories.Add(typeof(StartPage), new StartPageFactory(tagParserContainer, markdownWrapper));
        _factories.Add(typeof(ContactUsArea), new ContactUsAreaFactory(contactUsCategoryFactory));
        _factories.Add(typeof(List<Trivia>), triviaFactory);
    }

    public IProcessedContentType Build<T>(T content) => 
        _factories[content.GetType()].Build(content);
}