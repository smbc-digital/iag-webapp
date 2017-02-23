using StockportWebapp.Models;
using Xunit;

namespace StockportWebappTests.Unit.Model
{
    public class SubItemTest
    {
        [Fact]
        public void SetsNavigationLinkForATopic()
        {
            var subItem = new SubItem("slug", "title", "Teaser", "Icon", "topic", "image");

            Assert.Equal("/topic/slug", subItem.NavigationLink);
        }

        [Fact]
        public void SetsNavigationLinkForAnArticle()
        {
            var subItem = new SubItem("slug", "title", "Teaser", "Icon", "article", "image");

            Assert.Equal("/slug", subItem.NavigationLink);
        }

        [Fact]
        public void SetsNavigationLinkForAStartPage()
        {
            var subItem = new SubItem("slug", "title", "Teaser", "Icon", "start-page", "image");

            Assert.Equal("/start/slug", subItem.NavigationLink);
        }
    }
}