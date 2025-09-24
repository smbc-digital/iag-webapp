namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class SectionFactoryTest
{
    private readonly SectionFactory _factory;
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private readonly Mock<ITagParserContainer> _tagParserContainer = new();
    private readonly Section _section;
    private readonly Mock<IRepository> _repository = new();

    public SectionFactoryTest()
    {
        _factory = new SectionFactory(_tagParserContainer.Object, _markdownWrapper.Object, _repository.Object);
        _section = new Section("title",
                            "slug",
                            "Example meta description",
                            "The new content of the body",
                            new List<Profile>(),
                            new List<Document>(),
                            new List<Alert>(),
                            new List<TrustedLogo>(),
                            "logoAreaTitle",
                            DateTime.Now,
                            new List<InlineQuote>());

        _markdownWrapper
            .Setup(wrapper => wrapper.ConvertToHtml("The new content of the body"))
            .Returns("The new content of the body");

        _tagParserContainer
            .Setup(parser => parser.ParseAll("The new content of the body",
                                            It.IsAny<string>(),
                                            It.IsAny<bool>(),
                                            It.IsAny<IEnumerable<Alert>>(),
                                            It.IsAny<IEnumerable<Document>>(),
                                            It.IsAny<IEnumerable<InlineQuote>>(),
                                            It.IsAny<IEnumerable<PrivacyNotice>>(),
                                            It.IsAny<IEnumerable<Profile>>(),
                                            It.IsAny<IEnumerable<CallToActionBanner>>(),
                                            It.IsAny<bool>()))
            .Returns("The new content of the body");
        
        _repository
            .Setup(repo => repo.Get<List<PrivacyNotice>>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, new List<PrivacyNotice>(), string.Empty));
    }

    [Fact]
    public void ShouldSetTheCorrespondingFieldsForAProcessedSection()
    {
        // Act
        ProcessedSection result = _factory.Build(_section, "Article Title");

        // Assert
        result.Profiles.Should().BeEquivalentTo(new List<Profile>());
        Assert.Equal("The new content of the body", result.Body);
        Assert.Equal("title", result.Title);
        Assert.Equal("slug", result.Slug);
        Assert.Equal(new List<Profile>(), result.Profiles);
    }

    [Fact]
    public void ShouldProcessBodyWithMarkdown()
    {
        // Act & Assert
        _factory.Build(_section, "Article Title");
        _markdownWrapper.Verify(wrapper => wrapper.ConvertToHtml("The new content of the body"), Times.Once);
    }

    [Fact]
    public void ShouldProcessBodyWithTagParsing()
    {
        // Act & Assert
        _factory.Build(_section, "Article Title");
        _tagParserContainer.Verify(parser => parser.ParseAll("The new content of the body",
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
        _factory.Build(_section, "Article Title");
        _tagParserContainer.Verify(parser => parser.ParseAll("The new content of the body",
                                                            "Article Title",
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
        _factory.Build(_section, "Article Title");
        _tagParserContainer.Verify(parser => parser.ParseAll("The new content of the body",
                                                            "Article Title",
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