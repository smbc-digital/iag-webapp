namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class GroupHomepageFactoryTest
{
    private readonly Mock<MarkdownWrapper> _markdownWrapperMock = new();
    private readonly Mock<ITagParserContainer> _tagParserContainerMock = new();
    private readonly GroupHomepageFactory _groupHomepageFactory;
    private const string Title = "title";
    private readonly string Body = "body";
    private readonly GroupHomepage _groupHomepage;

    public GroupHomepageFactoryTest()
    {
        _groupHomepageFactory = new GroupHomepageFactory(_tagParserContainerMock.Object, _markdownWrapperMock.Object);

        _groupHomepage = new GroupHomepage
        {
            Title = Title,
            BackgroundImage = "background image",
            FeaturedGroupsHeading = string.Empty,
            FeaturedGroups = new List<Group>(),
            FeaturedGroupsCategory = new GroupCategory(),
            FeaturedGroupsSubCategory = new GroupSubCategory(),
            Alerts = new List<Alert>(),
            Body = "body",
            SecondaryBody = "secondary body",
            EventBanner = new EventBanner("title", "teaser", "icon", "link")
        };

        _markdownWrapperMock
            .Setup(wrapper => wrapper.ConvertToHtml(Body))
            .Returns(Body);

        _tagParserContainerMock
            .Setup(parser => parser.ParseAll(Body, It.IsAny<string>(), It.IsAny<bool>(), null, null, null, null, null, It.IsAny<bool>()))
            .Returns(Body);
    }

    [Fact]
    public void ItBuildsAGroupsHomepageWithProcessedBody()
    {
        // Act
        ProcessedGroupHomepage result = _groupHomepageFactory.Build(_groupHomepage);

        // Assert
        Assert.Equal(_groupHomepage.Title, result.Title);
        Assert.Equal(_groupHomepage.BackgroundImage, result.BackgroundImage);
        Assert.Equal(_groupHomepage.Body, result.Body);
    }

    [Fact]
    public void ShouldParseAllOfBody()
    {
        // Act
        ProcessedGroupHomepage result = _groupHomepageFactory.Build(_groupHomepage);

        // Assert
        _tagParserContainerMock.Verify(parser => parser.ParseAll(Body,
                                                                It.IsAny<string>(),
                                                                It.IsAny<bool>(),
                                                                null,
                                                                null,
                                                                null,
                                                                null,
                                                                null,
                                                                It.IsAny<bool>()), Times.Once);
                                                            
        _markdownWrapperMock.Verify(wrapper => wrapper.ConvertToHtml(Body), Times.Once);
    }
}