namespace StockportWebappTests_Unit.Unit.Parsers;

public class FormBuilderTagParserTests
{
    private readonly FormBuilderTagParser _parser = new();

    [Fact]
    public void Parse_Should_Parse_Form_Into_IFrame()
    {
        // Arrange
        string outputHtml = $"<iframe sandbox='allow-forms allow-scripts allow-top-navigation-by-user-activation allow-same-origin'  class='mapframe' allowfullscreen src='https://www.stockport.gov.uk/'></iframe>";

        // Act
        string response = _parser.Parse("{{FORM:https://www.stockport.gov.uk/}}");

        // Assert
        Assert.Equal(outputHtml, response);
    }

    [Fact]
    public void Parse_Should_Parse_Form_WithOptional_Iframe_Title()
    {
        // Arrange
        string outputHtml = $"<iframe sandbox='allow-forms allow-scripts allow-top-navigation-by-user-activation allow-same-origin' title=\"iframe optional title\" class='mapframe' allowfullscreen src='https://www.stockport.gov.uk/'></iframe>";

        // Act
        string response = _parser.Parse("{{FORM:https://www.stockport.gov.uk/;iframe optional title}}");

        // Assert
        Assert.Equal(outputHtml, response);
    }
}