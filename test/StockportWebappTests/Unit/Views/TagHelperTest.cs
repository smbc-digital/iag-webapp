
using FluentAssertions;
using Xunit;

using StockportWebapp.Models;

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
        public void RendersARazorTemplate()
        { 
            var html = renderer.RenderView("TestTemplates/StockButtons", "");

            html.Should().Contain("the button");
            html.Should().Contain("<button");
            html.Should().NotContain("stock-button-submit");
        }
    }

}
