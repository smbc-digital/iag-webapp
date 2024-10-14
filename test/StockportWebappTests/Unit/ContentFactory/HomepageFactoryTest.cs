namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class HomepageFactoryTest
{
    private readonly Mock<MarkdownWrapper> _markdownWrapperMock = new Mock<MarkdownWrapper>();

    private readonly HomepageFactory _homepageFactory;

    public HomepageFactoryTest()
    {
        _homepageFactory = new HomepageFactory(_markdownWrapperMock.Object);
    }

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
            freeText, null,
            string.Empty,
            string.Empty,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty),
            new CallToActionBanner(),
            new CallToActionBanner(),
            new List<SpotlightOnBanner>(),
            imageOverlayText);

        // Act
        var result = _homepageFactory.Build(homepage);

        // Assert
        result.FreeText.Should().Be(freeText);
        result.BackgroundImage.Should().Be(backgroundImage);
        _markdownWrapperMock.Verify(o => o.ConvertToHtml(freeText), Times.Once);
        _markdownWrapperMock.Verify(o => o.ConvertToHtml(imageOverlayText), Times.Once);
    }
}