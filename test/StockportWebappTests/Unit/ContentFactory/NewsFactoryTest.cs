namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class NewsFactoryTest
{
    private readonly NewsFactory _factory;
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private readonly Mock<ITagParserContainer> _tagParserContainer = new();
    private readonly News _news;
    private readonly List<Alert> _alerts = new() 
    {
        new Alert("Alert",
                "The Body",
                "Error",
                new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),
                string.Empty,
                false,
                string.Empty)
    };
    
    private readonly List<string> _tags = new() { "Events", "Bramall Hall" };
    private readonly List<Document> _documents = new();

    public NewsFactoryTest()
    {
        _factory = new NewsFactory(_tagParserContainer.Object, _markdownWrapper.Object);
        _news = new News("News 26th Aug",
                        "news-26th-aug",
                        "teaser",
                        "image",
                        "image",
                        "hero image caption",
                        "body",
                        new(2015, 9, 19),
                        "test",
                        new(2015, 9, 25),
                        new(2015, 9, 20),
                        _alerts,
                        _tags,
                        new List<InlineQuote>(),
                        null,
                        "logoAreaTitle",
                        new List<TrustedLogo>(),
                        null,
                        string.Empty,
                        null);
        
        _tagParserContainer
            .Setup(parser => parser.ParseAll("body",
                                            It.IsAny<string>(),
                                            It.IsAny<bool>(),
                                            null,
                                            It.IsAny<IEnumerable<Document>>(),
                                            null,
                                            null,
                                            It.IsAny<IEnumerable<Profile>>(),
                                            null,
                                            It.IsAny<bool>()))
            .Returns("body");
        
        _markdownWrapper
            .Setup(markdown => markdown.ConvertToHtml("body"))
            .Returns("body");
    }

    [Fact]
    public void Build_ShouldSetTheCorrespondingFieldsForProcessedNews()
    {
        // Act
        ProcessedNews result = _factory.Build(_news);

        // Assert
        Assert.Equal("News 26th Aug", result.Title);
        Assert.Equal("news-26th-aug", result.Slug);
        Assert.Equal("teaser", result.Teaser);
        Assert.Equal("image", result.Image);
        Assert.Equal("image", result.ThumbnailImage);
        Assert.Equal(new(2015, 9, 19), result.SunriseDate);
        Assert.Equal(new(2015, 9, 25), result.SunsetDate);
        Assert.Equal(new(2015, 9, 20), result.UpdatedAt);
        Assert.Equal(_alerts, result.Alerts);
        Assert.Equal(_tags, result.Tags);
    }

    [Fact]
    public void Build_ShouldProcessBodyWithMarkdown()
    {
        // Act & Assert
        _factory.Build(_news);
        _markdownWrapper.Verify(markdown => markdown.ConvertToHtml("body"), Times.Once);
    }

    [Fact]
    public void Build_ShouldProcessBodyWithTagParsing()
    {
        // Act & Assert
        _factory.Build(_news);
        _tagParserContainer.Verify(parser => parser.ParseAll("body",
                                                            _news.Title,
                                                            It.IsAny<bool>(),
                                                            It.IsAny<IEnumerable<Alert>>(),
                                                            null,
                                                            It.IsAny<IEnumerable<InlineQuote>>(),
                                                            It.IsAny<IEnumerable<PrivacyNotice>>(),
                                                            null,
                                                            null,
                                                            It.IsAny<bool>()),Times.Once);
    }
}