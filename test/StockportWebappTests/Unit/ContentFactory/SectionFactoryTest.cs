namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class SectionFactoryTest
{
    private readonly SectionFactory _factory;
    private readonly Mock<MarkdownWrapper> _markdownWrapper;
    private readonly Mock<ITagParserContainer> _tagParserContainer;
    private readonly Mock<IDynamicTagParser<Profile>> _profileTagParser;
    private readonly Mock<IDynamicTagParser<Document>> _documentTagParser;
    private readonly Mock<IDynamicTagParser<Alert>> _alertsInlineTagParser;
    private const string Title = "title";
    private const string Slug = "slug";
    private const string Body = "The new content of the body";
    private const string MetaDescription = "Example meta description";
    private readonly List<Profile> _profiles = new List<Profile>();
    private readonly List<Document> _documents = new List<Document>();
    private readonly Section _section;
    private readonly string _articleTitle = "Article Title";
    private readonly List<Alert> _emptyAlertsInline = new List<Alert>();
    private readonly Mock<IDynamicTagParser<PrivacyNotice>> _privacyNoticeTagParser;
    private readonly Mock<IRepository> _repository;


    public SectionFactoryTest()
    {
        _markdownWrapper = new Mock<MarkdownWrapper>();
        _tagParserContainer = new Mock<ITagParserContainer>();
        _profileTagParser = new Mock<IDynamicTagParser<Profile>>();
        _documentTagParser = new Mock<IDynamicTagParser<Document>>();
        _alertsInlineTagParser = new Mock<IDynamicTagParser<Alert>>();
        _privacyNoticeTagParser = new Mock<IDynamicTagParser<PrivacyNotice>>();
        _repository = new Mock<IRepository>();

        _factory = new SectionFactory(_tagParserContainer.Object, _profileTagParser.Object, _markdownWrapper.Object, _documentTagParser.Object, _alertsInlineTagParser.Object, _privacyNoticeTagParser.Object, _repository.Object);

        _section = new Section(Title, Slug, MetaDescription, Body, _profiles, _documents, _emptyAlertsInline);

        _markdownWrapper.Setup(o => o.ConvertToHtml(Body)).Returns(Body);
        _tagParserContainer.Setup(o => o.ParseAll(Body, It.IsAny<string>(), It.IsAny<bool>(), null, null, null, null, null)).Returns(Body);
        _profileTagParser.Setup(o => o.Parse(Body, _section.Profiles)).Returns(Body);
        _documentTagParser.Setup(o => o.Parse(Body, _section.Documents)).Returns(Body);
        _alertsInlineTagParser.Setup(o => o.Parse(Body, _emptyAlertsInline)).Returns(Body);
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

        _tagParserContainer.Verify(o => o.ParseAll(Body, It.IsAny<string>(), It.IsAny<bool>(), null, null, null, null, null), Times.Once);
    }

    [Fact]
    public void ShouldProcessBodyWithProfileTagParsing()
    {
        _factory.Build(_section, _articleTitle);

        _profileTagParser.Verify(o => o.Parse(Body, _section.Profiles), Times.Once);
    }

    [Fact]
    public void ShouldPassTitleToParserWhenBuilding()
    {
        _factory.Build(_section, _articleTitle);

        _tagParserContainer.Verify(o => o.ParseAll(Body, _articleTitle, It.IsAny<bool>(), null, null, null, null, null), Times.Once);
    }

    [Fact]
    public void Build_ShouldParseBodyIfPrivacyNoticeTagParserExists()
    {
        _section.Body = "{{PrivacyNotice:Births,deathsandmarriages}}";
        _factory.Build(_section);

        _privacyNoticeTagParser.Verify(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<PrivacyNotice>>()), Times.Once);
    }
}
