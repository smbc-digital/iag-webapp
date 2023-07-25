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
    public void ItBuildsAHomepageWithProcessedBody()
    {
        var freeText = "free text";
        _markdownWrapperMock.Setup(o => o.ConvertToHtml(freeText)).Returns(freeText);

        var backgroundImage = "background image";

        var homepage = new Homepage(Enumerable.Empty<string>(), string.Empty, string.Empty, new List<SubItem>(), new List<SubItem>(), new List<Alert>(), new List<CarouselContent>(), backgroundImage, freeText, null, string.Empty, string.Empty, new NullCarouselContent(), new CallToActionBanner());

        var result = _homepageFactory.Build(homepage);

        result.FreeText.Should().Be(freeText);
        result.BackgroundImage.Should().Be(backgroundImage);

        _markdownWrapperMock.Verify(o => o.ConvertToHtml(freeText), Times.Once);
    }
}