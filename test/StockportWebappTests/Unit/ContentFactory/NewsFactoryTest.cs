namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class NewsFactoryTest
{
    private readonly NewsFactory _factory;
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private readonly Mock<ITagParserContainer> _tagParserContainer = new();
    private readonly News _news;
    private const string Title = "News 26th Aug";
    private const string Slug = "news-26th-aug";
    private const string Teaser = "teaser";
    private const string Image = "image";
    private const string ThumbnailImage = "image";
    private const string Body = "body";
    private readonly DateTime _sunrise = new(2015, 9, 19);
    private readonly DateTime _sunset = new(2015, 9, 25);
    private readonly DateTime _updatedAt = new(2015, 9, 20);
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
    private readonly List<Profile> _profiles = new();

    public NewsFactoryTest()
    {
        _factory = new NewsFactory(_tagParserContainer.Object, _markdownWrapper.Object);
        _news = new News(Title,
                        Slug,
                        Teaser,
                        "hero image",
                        Image,
                        ThumbnailImage,
                        "hero image caption",
                        Body,
                        _sunrise,
                        "test",
                        _sunset,
                        _updatedAt,
                        _alerts,
                        _tags,
                        _documents,
                        _profiles,
                        new List<InlineQuote>(),
                        null,
                        "logoAreaTitle",
                        new List<TrustedLogo>(),
                        null,
                        string.Empty,
                        null);
        
        _tagParserContainer
            .Setup(parser => parser.ParseAll(Body,
                                            It.IsAny<string>(),
                                            It.IsAny<bool>(),
                                            null,
                                            It.IsAny<IEnumerable<Document>>(),
                                            null,
                                            null,
                                            It.IsAny<IEnumerable<Profile>>(),
                                            null,
                                            It.IsAny<bool>()))
            .Returns(Body);
        
        _markdownWrapper
            .Setup(markdown => markdown.ConvertToHtml(Body))
            .Returns(Body);
    }

    [Fact]
    public void Build_ShouldSetTheCorrespondingFieldsForProcessedNews()
    {
        // Act
        ProcessedNews result = _factory.Build(_news);

        // Assert
        Assert.Equal(Title, result.Title);
        Assert.Equal(Slug, result.Slug);
        Assert.Equal(Teaser, result.Teaser);
        Assert.Equal(Image, result.Image);
        Assert.Equal(ThumbnailImage, result.ThumbnailImage);
        Assert.Equal(_sunrise, result.SunriseDate);
        Assert.Equal(_sunset, result.SunsetDate);
        Assert.Equal(_updatedAt, result.UpdatedAt);
        Assert.Equal(_alerts, result.Alerts);
        Assert.Equal(_tags, result.Tags);
    }

    [Fact]
    public void Build_ShouldProcessBodyWithMarkdown()
    {
        // Act & Assert
        _factory.Build(_news);
        _markdownWrapper.Verify(markdown => markdown.ConvertToHtml(Body), Times.Once);
    }

    [Fact]
    public void Build_ShouldProcessBodyWithTagParsing()
    {
        // Act & Assert
        _factory.Build(_news);
        _tagParserContainer.Verify(parser => parser.ParseAll(Body,
                                                            _news.Title,
                                                            It.IsAny<bool>(),
                                                            It.IsAny<IEnumerable<Alert>>(),
                                                            _news.Documents,
                                                            It.IsAny<IEnumerable<InlineQuote>>(),
                                                            It.IsAny<IEnumerable<PrivacyNotice>>(),
                                                            _news.Profiles,
                                                            null,
                                                            It.IsAny<bool>()),Times.Once);
    }
}