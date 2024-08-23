namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class ShowcaseFactoryTest
{
    private readonly Mock<ITagParserContainer> _tagParserContainer;
    private readonly Mock<ITriviaFactory> _triviaFactory;
    private readonly Mock<MarkdownWrapper> _markdownWrapper;

    public ShowcaseFactoryTest()
    {
        _tagParserContainer = new Mock<ITagParserContainer>();
        _markdownWrapper = new Mock<MarkdownWrapper>();
        _triviaFactory = new Mock<ITriviaFactory>();
    }

    [Fact]
    public void ShouldSetTheCorrespondingFieldsForAProcessedShowcase()
    {
        // Arrange
        Showcase showcase = new ShowcaseBuilder()
            .Title("test title")
            .Slug("test_slug")
            .Teaser("test teaser")
            .MetaDescription("test metaDescription")
            .Subheading("test subheading")
            .HeroImageUrl("test-image-url.jpg")
            .Body("body")
            .Breadcrumbs(new List<Crumb> { new Crumb("test link", "test title", "test type") })
            .FeaturedItems(new List<SubItem>
                {
                    new("slug","title", "icon", "teaser", "link", "contentType", "image-url.jpg", 0, "body text", new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty)
                })
            .Build();

        ShowcaseFactory _showcaseFactory = new(_tagParserContainer.Object, _markdownWrapper.Object, _triviaFactory.Object);

        // Act
        ProcessedShowcase processedShowcase = _showcaseFactory.Build(showcase);

        // Assert   
        processedShowcase.Should().BeEquivalentTo(showcase);
    }
}