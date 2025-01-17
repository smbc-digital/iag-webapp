namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class TopicFactoryTest
{
    private readonly Mock<ITagParserContainer> _tagParserContainer = new();
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private readonly TopicFactory _topicFactory;
    private readonly Topic _topic;
    private const string Slug = "slug";
    private const string Summary = "summary";
    private const string Teaser = "teaser";
    private const string MetaDescription = "meta desctiption";
    private const string Icon = "icon";
    private const string Image = "Image";
    private const string BackgroundImage = "backgroundimage.jpg";
    private readonly List<Crumb> _breadcrumbs = new();
    private readonly List<SubItem> _featuredTasks = new();
    private readonly List<SubItem> _subItems = new();
    private readonly List<SubItem> _secondaryItems = new();
    private readonly EventCalendarBanner _eventCalendarBanner = new() {
        Title = "title",
        Teaser = "teaser",
        Link = "link",
        Icon = "icon",
        Colour = EColourScheme.Teal
    };

    public TopicFactoryTest()
    {
        _topicFactory = new TopicFactory(_tagParserContainer.Object, _markdownWrapper.Object);

        _topic = new Topic("name",
                        Slug,
                        Summary,
                        Teaser,
                        MetaDescription,
                        Icon,
                        BackgroundImage,
                        Image,
                        _featuredTasks,
                        _subItems,
                        _secondaryItems,
                        _breadcrumbs,
                        new List<Alert>(),
                        false,
                        "emailAlertsTopic",
                        _eventCalendarBanner,
                        true,
                        new CarouselContent("Title", "Teaser", "Image", "url", new DateTime()),
                        "event Category",
                        null,
                        string.Empty)
        {
            Video = new()
        };

        _markdownWrapper.Setup(_ => _.ConvertToHtml(Summary)).Returns(Summary);
        _tagParserContainer
            .Setup(parser => parser.ParseAll(Summary, "name", It.IsAny<bool>(), null, null, null, null, null, null, It.IsAny<bool>()))
            .Returns(Summary);
    }

    [Fact]
    public void Build_ShouldSetTheCorrespondingFields_For_AProcessedTopic()
    {
        // Act
        ProcessedTopic result = _topicFactory.Build(_topic);

        // Assert
        Assert.Equal("name", result.Name);
        Assert.Equal("/topic/" + Slug, result.NavigationLink);
        Assert.Equal(Summary, result.Summary);
        Assert.Equal(Teaser, result.Teaser);
        Assert.Equal(MetaDescription, result.MetaDescription);
        Assert.Equal(Icon, result.Icon);
        Assert.Equal(Image, result.Image);
        Assert.Equal(BackgroundImage, result.BackgroundImage);
        Assert.Equal(_breadcrumbs, result.Breadcrumbs.ToList());
        Assert.Equal(_subItems, result.SubItems.ToList());
        Assert.Equal(_secondaryItems, result.SecondaryItems.ToList());
    }

    [Fact]
    public void Build_ShouldProcessSummaryWithMarkdown()
    {
        // Act
        _topicFactory.Build(_topic);

        // Assert
        _markdownWrapper.Verify(_ => _.ConvertToHtml(Summary), Times.Once);
    }

    [Fact]
    public void Build_ShouldProcessSummaryWithStaticTagParsing()
    {
        // Act
        _topicFactory.Build(_topic);

        // Assert
        _tagParserContainer.Verify(parser => parser.ParseAll(Summary,
                                                            "name",
                                                            It.IsAny<bool>(),
                                                            null,
                                                            null,
                                                            null,
                                                            null,
                                                            null,
                                                            null,
                                                            It.IsAny<bool>()), Times.Once);
    }
}