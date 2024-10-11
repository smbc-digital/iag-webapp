namespace StockportWebappTests_Unit.Unit.Parsers;

public class MapTagParserTests
{
    private readonly Mock<IViewRender> _mockViewRender;
    private readonly MapTagParser _parser;

    public MapTagParserTests()
    {
        _mockViewRender = new Mock<IViewRender>();
        _parser = new MapTagParser(_mockViewRender.Object);
    }

    [Fact]
    public void GenerateHtml_ShouldReturnRenderedHtml_WithExpectedTag()
    {
        // Arrange
        string tagData = "{{MAP:url}}";
        string expectedTagData = "url";
        string expectedHtml = "<div>Map Content</div>";
        _mockViewRender.Setup(o => o.Render("MapContent", It.IsAny<MapViewModel>())).Returns(expectedHtml);

        // Act
        string result = _parser.GenerateHtml(tagData);

        // Assert
        Assert.Equal(expectedHtml, result);
        _mockViewRender.Verify(o => o.Render("MapContent", It.Is<MapViewModel>(model => model.TagData.Equals(expectedTagData))), Times.Once);
    }
}