﻿namespace StockportWebappTests_Unit.Unit.ContentFactory;

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
        var freeText = "free text";
        _markdownWrapperMock.Setup(o => o.ConvertToHtml(freeText)).Returns(freeText);

        var backgroundImage = "background image";
        var foregroundImage = "foreground image";

        var homepage = new Homepage("Test", Enumerable.Empty<string>(), string.Empty, string.Empty, new List<SubItem>(), new List<SubItem>(), new List<Alert>(), new List<CarouselContent>(), backgroundImage, foregroundImage, string.Empty, string.Empty, string.Empty, freeText, null, string.Empty, string.Empty, new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), new CallToActionBanner(), new CallToActionBanner(), new List<SpotlightOnBanner>());

        // Act
        var result = _homepageFactory.Build(homepage);

        // Assert
        result.FreeText.Should().Be(freeText);
        result.BackgroundImage.Should().Be(backgroundImage);
        _markdownWrapperMock.Verify(o => o.ConvertToHtml(freeText), Times.Once);
    }
}