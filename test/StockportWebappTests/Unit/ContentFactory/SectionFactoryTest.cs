namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class SectionFactoryTest
{
    private readonly SectionFactory _factory;
    private readonly Mock<MarkdownWrapper> _markdownWrapper;
    private readonly Mock<ITagParserContainer> _tagParserContainer;
    private const string Title = "title";
    private const string Slug = "slug";
    private const string Body = "The new content of the body";
    private const string MetaDescription = "Example meta description";
    private readonly List<Profile> _profiles = new();
    private readonly List<Document> _documents = new();
    private readonly Section _section;
    private readonly string _articleTitle = "Article Title";
    private readonly List<Alert> _emptyAlertsInline = new();
    private readonly List<GroupBranding> _sectionBranding = new();
    private const string _logoAreaTitle = "logoAreaTitle";
    private readonly DateTime _updatedAt = DateTime.Now;
    private readonly Mock<IRepository> _repository;

    public SectionFactoryTest()
    {
        _markdownWrapper = new Mock<MarkdownWrapper>();
        _tagParserContainer = new Mock<ITagParserContainer>();
        _repository = new Mock<IRepository>();

        _factory = new SectionFactory(_tagParserContainer.Object, _markdownWrapper.Object, _repository.Object);

        _section = new Section(Title, Slug, MetaDescription, Body, _profiles, _documents, _emptyAlertsInline, _sectionBranding, _logoAreaTitle, _updatedAt);

        _markdownWrapper.Setup(o => o.ConvertToHtml(Body)).Returns(Body);
        _tagParserContainer.Setup(o => o.ParseAll(Body, It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IEnumerable<Alert>>(), It.IsAny<IEnumerable<Document>>(), It.IsAny<IEnumerable<InlineQuote>>(),
                It.IsAny<IEnumerable<PrivacyNotice>>(), It.IsAny<IEnumerable<Profile>>(), It.IsAny<bool>())).Returns(Body);
        _repository.Setup(o => o.Get<List<PrivacyNotice>>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, new List<PrivacyNotice>(), ""));
    }

    [Fact]
    public void ShouldSetTheCorrespondingFieldsForAProcessedSection()
    {
        var result = _factory.Build(_section, _articleTitle);

        result.Body.Should().Be(Body);
        result.Title.Should().Be(Title);
        result.Slug.Should().Be(Slug);
        result.Profiles.Should().BeEquivalentTo(_profiles);
    }

    [Fact]
    public void ShouldProcessBodyWithMarkdown()
    {
        _factory.Build(_section, _articleTitle);

        _markdownWrapper.Verify(o => o.ConvertToHtml(Body), Times.Once);
    }

    [Fact]
    public void ShouldProcessBodyWithTagParsing()
    {
        _factory.Build(_section, _articleTitle);

        _tagParserContainer.Verify(o => o.ParseAll(Body, It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IEnumerable<Alert>>(), It.IsAny<IEnumerable<Document>>(), It.IsAny<IEnumerable<InlineQuote>>(),
                It.IsAny<IEnumerable<PrivacyNotice>>(), _section.Profiles, It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public void ShouldProcessBodyWithProfileTagParsing()
    {
        _factory.Build(_section, _articleTitle);

        _tagParserContainer.Verify(o => o.ParseAll(Body, _articleTitle, It.IsAny<bool>(), It.IsAny<IEnumerable<Alert>>(), It.IsAny<IEnumerable<Document>>(), It.IsAny<IEnumerable<InlineQuote>>(),
                It.IsAny<IEnumerable<PrivacyNotice>>(), _section.Profiles, It.IsAny<bool>()), Times.Once); 
    }

    [Fact]
    public void ShouldPassTitleToParserWhenBuilding()
    {
        _factory.Build(_section, _articleTitle);

        _tagParserContainer.Verify(o => o.ParseAll(Body, _articleTitle, It.IsAny<bool>(), It.IsAny<IEnumerable<Alert>>(), It.IsAny<IEnumerable<Document>>(), It.IsAny<IEnumerable<InlineQuote>>(),
                It.IsAny<IEnumerable<PrivacyNotice>>(), It.IsAny<IEnumerable<Profile>>(), It.IsAny<bool>()), Times.Once); 
    }
}
