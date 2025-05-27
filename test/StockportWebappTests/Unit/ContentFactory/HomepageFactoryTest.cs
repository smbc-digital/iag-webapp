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
        string freeText = "free text";
        _markdownWrapperMock
            .Setup(markdownWrapper => markdownWrapper.ConvertToHtml(freeText))
            .Returns(freeText);

        string imageOverlayText = "image overlay text";
        _markdownWrapperMock
            .Setup(markdownWrapper => markdownWrapper.ConvertToHtml(imageOverlayText))
            .Returns(imageOverlayText);

        string backgroundImage = "background image";
        string foregroundImage = "foreground image";

        Homepage homepage = new("Test",
            Enumerable.Empty<string>(),
            string.Empty,
            string.Empty,
            new List<SubItem>(),
            new List<SubItem>(),
            new List<Alert>(),
            new List<CarouselContent>(),
            backgroundImage,
            foregroundImage,
            string.Empty,
            string.Empty,
            string.Empty,
            freeText,
            string.Empty,
            string.Empty,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty, new DateTime()),
            new CallToActionBanner(),
            new CallToActionBanner(),
            new List<SpotlightOnBanner>(),
            imageOverlayText);

        // Act
        ProcessedHomepage result = _homepageFactory.Build(homepage);

        // Assert
        Assert.Equal(freeText, result.FreeText);
        Assert.Equal(backgroundImage, result.BackgroundImage);
        _markdownWrapperMock.Verify(wrapper => wrapper.ConvertToHtml(freeText), Times.Once);
        _markdownWrapperMock.Verify(wrapper => wrapper.ConvertToHtml(imageOverlayText), Times.Once);
    }
}