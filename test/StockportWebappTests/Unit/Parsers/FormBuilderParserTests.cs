using FluentAssertions;
using StockportWebapp.Parsers;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Parsers
{
    public class FormBuilderTagParserTests
    {
        private readonly FormBuilderTagParser _parser;

        public FormBuilderTagParserTests()
        {
            _parser = new FormBuilderTagParser();
        }

        [Fact]
        public void Parse_Should_Parse_Form_Into_IFrame()
        {
            var tag = "https://www.stockport.gov.uk/";
            var response = _parser.Parse("{{FORM:" + tag + "}}");

            var outputHtml = $"<iframe sandbox='allow-forms allow-scripts allow-top-navigation-by-user-activation allow-same-origin'  class='mapframe' allowfullscreen src='https://www.stockport.gov.uk/'></iframe>";

            response.Should().Be(outputHtml);
        }

        [Fact]
        public void Parse_Should_Parse_Form_WithOptional_Iframe_Title()
        {
            var tag = "https://www.stockport.gov.uk/;iframe optional title";
            var response = _parser.Parse("{{FORM:" + tag + "}}");

            var outputHtml = $"<iframe sandbox='allow-forms allow-scripts allow-top-navigation-by-user-activation allow-same-origin' title=\"iframe optional title\" class='mapframe' allowfullscreen src='https://www.stockport.gov.uk/'></iframe>";

            response.Should().Be(outputHtml);
        }
    }
}
