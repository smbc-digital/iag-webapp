using StockportWebapp.TagParsers;

namespace StockportWebappTests_Unit.Unit.Parsers;

public class IFrameTagParserTests
{
    private readonly IFrameTagParser _parser;

    public IFrameTagParserTests()
    {
        _parser = new IFrameTagParser();
    }

    [Fact]
    public void Parse_Should_Parse_IFrame()
    {
        var tag = "https://www.stockport.gov.uk/";
        var response = _parser.Parse("{{IFRAME:" + tag + "}}");

        var outputHtml = $"<iframe  class='mapframe' allowfullscreen src='https://www.stockport.gov.uk/'></iframe>";

        response.Should().Be(outputHtml);
    }

    [Fact]
    public void Parse_Should_Parse_IFrame_WithOptional_Iframe_Title()
    {
        var tag = "https://www.stockport.gov.uk/;iframe optional title";
        var response = _parser.Parse("{{IFRAME:" + tag + "}}");

        var outputHtml = $"<iframe title=\"iframe optional title\" class='mapframe' allowfullscreen src='https://www.stockport.gov.uk/'></iframe>";

        response.Should().Be(outputHtml);
    }
}
