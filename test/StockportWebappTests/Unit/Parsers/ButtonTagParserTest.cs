namespace StockportWebappTests_Unit.Unit.Parsers;

public class ButtonTagParserTest
{
    private readonly ButtonTagParser _buttonParser = new();

    [Fact]
    public void ItParsesTheButtonTagAndReplacesItWithHtmlButtonWithTitleIfTitleIsGiven()
    {
        // Arrange
        string expectedHtmlData = HtmlButton("http://www.example.com", "Click here!");

        // Act
        string result = _buttonParser.Parse("{{BUTTON: http://www.example.com, Click here!}}");

        // Assert
        Assert.Equal(expectedHtmlData, result);
    }

    [Fact]
    public void ItParsesTheButtonTagWithRelativeLinks()
    {
        // Arrange
        string expectedHtmlData = HtmlButton("/relative_link/to_the_website/", "Click here!");
        
        // Act
        string result = _buttonParser.Parse("{{BUTTON: /relative_link/to_the_website/, Click here!}}");

        // Assert
        Assert.Equal(expectedHtmlData, result);
    }

    [Fact]
    public void ItParsesTheButtonTagAndReplacesItWithHtmlButtonWithLinkAsTitleIfNoTitleIsGiven()
    {
        // Arrange
        string expectedHtmlData = HtmlButton("http://www.no-title-here.com");

        // Act
        string result = _buttonParser.Parse("{{BUTTON: http://www.no-title-here.com}}");

        // Assert
        Assert.Equal(expectedHtmlData, result);
    }

    [Theory]
    [InlineData("http://www.google.com, Okay Google", "http://www.google.com", "Okay Google")]
    [InlineData("http://www.stockport.gov.uk, Hello, MyAccount", "http://www.stockport.gov.uk", "Hello, MyAccount")]
    [InlineData("http://www.stockport.gov.uk, Hello, React, Forms", "http://www.stockport.gov.uk", "Hello, React, Forms")]
    protected void ItParsesButtonTagAndSplitsLinkAndTitleWithComma(string tagData, string expectedButtonLink, string expectedButtonTitle)
    {
        // Arrange
        string expectedHtmlData = HtmlButton(expectedButtonLink, expectedButtonTitle);

        // Act
        string result = _buttonParser.Parse($"{{{{BUTTON: {tagData}}}}}");

        // Assert
        Assert.Equal(expectedHtmlData, result);
    }


    [Fact]
    public void ItParsesAllButtonTagsAndReplacesThemWithHtmlButtons()
    {
        // Arrange
        string expectedHtmlData = $"{HtmlButton("http://www.example1.com", "Click")} body text {HtmlButton("http://www.example2.com", "Here")}";
        
        // Act
        string result = _buttonParser.Parse("{{BUTTON: http://www.example1.com, Click}} body text {{BUTTON: http://www.example2.com, Here}}");
        
        // Assert
        Assert.Equal(expectedHtmlData, result);
    }
    private static string HtmlButton(string link) =>
        HtmlButton(link, link);

    private static string HtmlButton(string link, string title) =>
        $"<a class=\"btn button button-hs button-primary button-outline button-partialrounded btn--chevron-forward\" href=\"{link}\">{title}</a>";
}