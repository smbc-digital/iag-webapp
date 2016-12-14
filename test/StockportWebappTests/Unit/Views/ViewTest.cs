
using FluentAssertions;
using Xunit;

using StockportWebapp.Models;

namespace StockportWebappTests.Unit.Views
{
    public class ViewTest
    {
        private TestViewRenderer renderer;

        public ViewTest()
        {
            renderer = new TestViewRenderer("src", "StockportWebapp");
        }

        [Fact]
        public void RendersARazorTemplate()
        { 
            var html = renderer.RenderView("DisplayTemplates/Alert", new Alert("title", "sub", "the body", "Warning"));

            html.Should().Contain("the body");
        }
    }

}
