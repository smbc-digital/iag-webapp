namespace StockportWebappTests_Unit.Unit.Parsers;

public class MapTagParserTests
{
    private readonly Mock<IViewRender> _mockViewRender = new();
    private readonly MapTagParser _parser;

    public MapTagParserTests() =>
        _parser = new(_mockViewRender.Object);

    [Fact]
    public void GenerateHtml_ShouldReturnRenderedHtml_WithExpectedTag()
    {
        // Arrange
        _mockViewRender
            .Setup(renderer => renderer.Render("MapContent", It.IsAny<MapViewModel>()))
            .Returns("<div>Map Content</div>");

        // Act
        string result = _parser.GenerateHtml("{{MAP:url}}");

        // Assert
        Assert.Equal("<div>Map Content</div>", result);
        _mockViewRender.Verify(renderer => renderer.Render("MapContent", It.Is<MapViewModel>(model => model.TagData.Equals("url"))), Times.Once);
    }
}