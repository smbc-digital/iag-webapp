namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class SectionFactoryTest
{
    private readonly SectionFactory _factory;
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private readonly Mock<ITagParserContainer> _tagParserContainer = new();
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
    private readonly Mock<IRepository> _repository = new();

    public SectionFactoryTest()
    {
        _factory = new SectionFactory(_tagParserContainer.Object, _markdownWrapper.Object, _repository.Object);
        _section = new Section(Title,
                            Slug,
                            MetaDescription,
                            Body,
                            _profiles,
                            _documents,
                            _emptyAlertsInline,
                            _sectionBranding,
                            _logoAreaTitle,
                            _updatedAt);

        _markdownWrapper.Setup(wrapper => wrapper.ConvertToHtml(Body)).Returns(Body);

        _tagParserContainer
            .Setup(parser => parser.ParseAll(Body,
                                            It.IsAny<string>(),
                                            It.IsAny<bool>(),
                                            It.IsAny<IEnumerable<Alert>>(),
                                            It.IsAny<IEnumerable<Document>>(),
                                            It.IsAny<IEnumerable<InlineQuote>>(),
                                            It.IsAny<IEnumerable<PrivacyNotice>>(),
                                            It.IsAny<IEnumerable<Profile>>(),
                                            It.IsAny<IEnumerable<CallToActionBanner>>(),
                                            It.IsAny<bool>()))
            .Returns(Body);
        
        _repository
            .Setup(repo => repo.Get<List<PrivacyNotice>>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, new List<PrivacyNotice>(), string.Empty));
    }

    [Fact]
    public void ShouldSetTheCorrespondingFieldsForAProcessedSection()
    {
        // Act
        ProcessedSection result = _factory.Build(_section, _articleTitle);

        // Assert
        result.Profiles.Should().BeEquivalentTo(_profiles);
        Assert.Equal(Body, result.Body);
        Assert.Equal(Title, result.Title);
        Assert.Equal(Slug, result.Slug);
        Assert.Equal(_profiles, result.Profiles);
    }

    [Fact]
    public void ShouldProcessBodyWithMarkdown()
    {
        // Act & Assert
        _factory.Build(_section, _articleTitle);
        _markdownWrapper.Verify(wrapper => wrapper.ConvertToHtml(Body), Times.Once);
    }

    [Fact]
    public void ShouldProcessBodyWithTagParsing()
    {
        // Act & Assert
        _factory.Build(_section, _articleTitle);
        _tagParserContainer.Verify(parser => parser.ParseAll(Body,
                                                            It.IsAny<string>(),
                                                            It.IsAny<bool>(),
                                                            It.IsAny<IEnumerable<Alert>>(),
                                                            It.IsAny<IEnumerable<Document>>(),
                                                            It.IsAny<IEnumerable<InlineQuote>>(),
                                                            It.IsAny<IEnumerable<PrivacyNotice>>(),
                                                            _section.Profiles,
                                                            null,
                                                            It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public void ShouldProcessBodyWithProfileTagParsing()
    {
        // Act & Assert
        _factory.Build(_section, _articleTitle);
        _tagParserContainer.Verify(parser => parser.ParseAll(Body,
                                                            _articleTitle,
                                                            It.IsAny<bool>(),
                                                            It.IsAny<IEnumerable<Alert>>(),
                                                            It.IsAny<IEnumerable<Document>>(),
                                                            It.IsAny<IEnumerable<InlineQuote>>(),
                                                            It.IsAny<IEnumerable<PrivacyNotice>>(),
                                                            _section.Profiles,
                                                            null,
                                                            It.IsAny<bool>()), Times.Once); 
    }

    [Fact]
    public void ShouldPassTitleToParserWhenBuilding()
    {
        // Act & Assert
        _factory.Build(_section, _articleTitle);
        _tagParserContainer.Verify(parser => parser.ParseAll(Body,
                                                _articleTitle,
                                                It.IsAny<bool>(),
                                                It.IsAny<IEnumerable<Alert>>(),
                                                It.IsAny<IEnumerable<Document>>(),
                                                It.IsAny<IEnumerable<InlineQuote>>(),
                                                It.IsAny<IEnumerable<PrivacyNotice>>(),
                                                It.IsAny<IEnumerable<Profile>>(),
                                                It.IsAny<IEnumerable<CallToActionBanner>>(),
                                                It.IsAny<bool>()), Times.Once); 
    }
}