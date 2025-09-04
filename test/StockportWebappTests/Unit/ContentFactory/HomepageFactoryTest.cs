namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class HomepageFactoryTest
{
    private readonly Mock<MarkdownWrapper> _markdownWrapperMock = new();
    private readonly HomepageFactory _homepageFactory;

    public HomepageFactoryTest() =>
        _homepageFactory = new HomepageFactory(_markdownWrapperMock.Object);

    [Fact]
    public void Build_ItBuildsAHomepageWithProcessedBody()
    {
        // Arrange
        _markdownWrapperMock
            .Setup(markdownWrapper => markdownWrapper.ConvertToHtml("free text"))
            .Returns("free text");

        _markdownWrapperMock
            .Setup(markdownWrapper => markdownWrapper.ConvertToHtml("image overlay text"))
            .Returns("image overlay text");

        Homepage homepage = new("Test",
                                string.Empty,
                                string.Empty,
                                new List<SubItem>(),
                                new List<SubItem>(),
                                new List<Alert>(),
                                new List<CarouselContent>(),
                                "background image",
                                "foreground image",
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                "free text",
                                string.Empty,
                                string.Empty,
                                new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty, new DateTime()),
                                new CallToActionBanner(),
                                new CallToActionBanner(),
                                new List<SpotlightOnBanner>(),
                                "image overlay text");

        // Act
        ProcessedHomepage result = _homepageFactory.Build(homepage);

        // Assert
        Assert.Equal("free text", result.FreeText);
        Assert.Equal("background image", result.BackgroundImage);
        _markdownWrapperMock.Verify(wrapper => wrapper.ConvertToHtml("free text"), Times.Once);
        _markdownWrapperMock.Verify(wrapper => wrapper.ConvertToHtml("image overlay text"), Times.Once);
    }
}