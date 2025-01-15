namespace StockportWebappTests_Unit.Unit.Parsers;

public class IFrameTagParserTests
{
    private readonly IFrameTagParser _parser = new();

    [Fact]
    public void Parse_Should_Parse_IFrame()
    {
        // Arrange
        string outputHtml = $"<iframe  class='mapframe' allowfullscreen src='https://www.stockport.gov.uk/'></iframe>";

        // Act
        string response = _parser.Parse("{{IFRAME:https://www.stockport.gov.uk/}}");

        // Assert
        Assert.Equal(outputHtml, response);
    }

    [Fact]
    public void Parse_Should_Parse_IFrame_WithOptional_Iframe_Title()
    {
        // Arrange
        string outputHtml = $"<iframe title=\"iframe optional title\" class='mapframe' allowfullscreen src='https://www.stockport.gov.uk/'></iframe>";

        // Act
        string response = _parser.Parse("{{IFRAME:https://www.stockport.gov.uk/;iframe optional title}}");

        // Assert
        Assert.Equal(outputHtml, response);
    }
}