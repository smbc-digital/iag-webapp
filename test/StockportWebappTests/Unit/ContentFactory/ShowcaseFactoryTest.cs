namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class ShowcaseFactoryTest
{
    private readonly Mock<ITagParserContainer> _tagParserContainer = new();
    private readonly Mock<ITriviaFactory> _triviaFactory = new();
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();

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
            .Breadcrumbs(new List<Crumb> { new("test link", "test title", "test type") })
            .FeaturedItems(new List<SubItem>
                {
                    new("slug","title", "icon", "teaser", "teaser image", "link", "image-url.jpg", new List<SubItem>(), EColourScheme.Teal)
                })
            .Build();

        ShowcaseFactory _showcaseFactory = new(_tagParserContainer.Object, _markdownWrapper.Object, _triviaFactory.Object);

        // Act
        ProcessedShowcase processedShowcase = _showcaseFactory.Build(showcase);

        // Assert   
        processedShowcase.Should().BeEquivalentTo(showcase);
    }
}