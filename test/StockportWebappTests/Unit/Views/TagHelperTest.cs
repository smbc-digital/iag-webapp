
using FluentAssertions;
using Xunit;

using StockportWebapp.Models;
using System.IO;

namespace StockportWebappTests.Unit.Views
{
    public class TagHelperTest
    {
        private TestViewRenderer renderer;

        public TagHelperTest()
        {
            renderer = new TestViewRenderer("test", "StockportWebappTests");
        }

        [Fact]
        public void RendersFromARazorTemplateFile()
        { 
            var html = renderer.RenderView("TestTemplates/StockButtons", "");

            html.Should().Contain("the button");
            html.Should().Contain("<button");
            html.Should().NotContain("stock-button-submit");
        }

        [Fact]
        public void RenderTheStockSubmitButton()
        { 
            var html = Render("<stock-button-submit>another button</stock-button-submit>");

            html.Should().Be("<button class=\"button-default\">another button</button>");
        }

        [Fact]
        public void RenderTheStockSubmitButtonAsALink()
        { 
            var html = Render("<stock-button-submit as-link=\"true\" href=\"/the-link\">link</stock-button-submit>");

            html.Should().Be("<a href=\"/the-link\" class=\"button-default\">link</a>");
        }

        private string Render(string template)
        {
            var fullTemplate = "@addTagHelper \"*, Microsoft.AspNetCore.Mvc.TagHelpers\"\n" +
                               "@addTagHelper \"*, StockportWebApp\"\n" + template;
            File.WriteAllText("Views/TestTemplates/Generated/TestTemplate.cshtml", fullTemplate);
            var html = renderer.RenderView("TestTemplates/Generated/TestTemplate", "");
            return html;
        }
    }

}
