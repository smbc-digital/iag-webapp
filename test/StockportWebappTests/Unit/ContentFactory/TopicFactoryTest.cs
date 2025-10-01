namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class TopicFactoryTest
{
    private readonly Mock<ITagParserContainer> _tagParserContainer = new();
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private readonly TopicFactory _topicFactory;
    private readonly Topic _topic;
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
                        "slug",
                        "summary",
                        "teaser",
                        "meta desctiption",
                        "icon",
                        "backgroundimage.jpg",
                        "Image",
                        new List<SubItem>(),
                        new List<SubItem>(),
                        new List<SubItem>(),
                        new List<Crumb>(),
                        new List<Alert>(),
                        _eventCalendarBanner,
                        true,
                        new CarouselContent("Title", "Teaser", "Image", "url", new DateTime()),
                        "event Category",
                        null,
                        string.Empty)
        {
            Video = new()
        };

        _markdownWrapper
            .Setup(markdownWrapper => markdownWrapper.ConvertToHtml("summary"))
            .Returns("summary");

        _tagParserContainer
            .Setup(parser => parser.ParseAll("summary", "name", It.IsAny<bool>(), null, null, null, null, null, null, It.IsAny<bool>()))
            .Returns("summary");
    }

    [Fact]
    public void Build_ShouldSetTheCorrespondingFields_For_AProcessedTopic()
    {
        // Act
        ProcessedTopic result = _topicFactory.Build(_topic);

        // Assert
        Assert.Equal("name", result.Title);
        Assert.Equal("/topic/slug", result.NavigationLink);
        Assert.Equal("summary", result.Summary);
        Assert.Equal("teaser", result.Teaser);
        Assert.Equal("meta desctiption", result.MetaDescription);
        Assert.Equal("icon", result.Icon);
        Assert.Equal("Image", result.Image);
        Assert.Equal("backgroundimage.jpg", result.BackgroundImage);
        Assert.Equal(new List<Crumb>(), result.Breadcrumbs.ToList());
        Assert.Equal(new List<SubItem>(), result.SubItems.ToList());
        Assert.Equal(new List<SubItem>(), result.SecondaryItems.ToList());
    }

    [Fact]
    public void Build_ShouldProcessSummaryWithMarkdown()
    {
        // Act
        _topicFactory.Build(_topic);

        // Assert
        _markdownWrapper.Verify(markdownWrapper => markdownWrapper.ConvertToHtml("summary"), Times.Once);
    }

    [Fact]
    public void Build_ShouldProcessSummaryWithStaticTagParsing()
    {
        // Act
        _topicFactory.Build(_topic);

        // Assert
        _tagParserContainer.Verify(parser => parser.ParseAll("summary",
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