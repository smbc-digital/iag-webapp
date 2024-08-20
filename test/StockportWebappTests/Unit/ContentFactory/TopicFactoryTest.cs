namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class TopicFactoryTest
{
    private readonly Mock<ITagParserContainer> _tagParserContainer;
    private readonly Mock<MarkdownWrapper> _markdownWrapper;
    private readonly TopicFactory _topicFactory;
    private readonly Topic _topic;
    private const string Title = "title";
    private const string Slug = "slug";
    private const string Summary = "summary";
    private const string Teaser = "teaser";
    private const string MetaDescription = "meta desctiption";
    private const string Icon = "icon";
    private const string Image = "Image";
    private const string BackgroundImage = "backgroundimage.jpg";
    private readonly List<Crumb> _breadcrumbs;
    private readonly List<SubItem> _featuredTasks;
    private readonly List<SubItem> _subItems;
    private readonly List<SubItem> _secondaryItems;
    private readonly EventCalendarBanner _eventCalendarBanner = new() {
        Title = "title",
        Teaser = "teaser",
        Link = "link",
        Icon = "icon",
        Colour = EColourScheme.Teal
    };

    public TopicFactoryTest()
    {
        _tagParserContainer = new Mock<ITagParserContainer>();
        _markdownWrapper = new Mock<MarkdownWrapper>();
        _topicFactory = new TopicFactory(_tagParserContainer.Object, _markdownWrapper.Object);
        _breadcrumbs = new List<Crumb>();

        _featuredTasks = new List<SubItem>();
        _subItems = new List<SubItem>();
        _secondaryItems = new List<SubItem>();

        _topic = new Topic("name", Slug, Summary, Teaser, MetaDescription, Icon, BackgroundImage, Image, _featuredTasks, _subItems, _secondaryItems, _breadcrumbs,
            new List<Alert>(), false, "emailAlertsTopic", _eventCalendarBanner,
            Title, true, new CarouselContent("Title", "Teaser", "Image", "url"),
            "event Category", null, string.Empty)
        {
            Video = new()
        };

        _markdownWrapper.Setup(_ => _.ConvertToHtml(Summary)).Returns(Summary);
        _tagParserContainer.Setup(_ => _.ParseAll(Summary, Title, It.IsAny<bool>(), null, null, null, null, null, It.IsAny<bool>())).Returns(Summary);
    }

    [Fact]
    public void Build_ShouldSetTheCorrespondingFields_For_AProcessedTopic()
    {
        // Act
        ProcessedTopic result = _topicFactory.Build(_topic);

        // Assert
        Assert.Equal(Title, result.Title);
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
        _tagParserContainer.Verify(_ => _.ParseAll(Summary, Title, It.IsAny<bool>(), null, null, null, null, null, It.IsAny<bool>()), Times.Once);
    }
}