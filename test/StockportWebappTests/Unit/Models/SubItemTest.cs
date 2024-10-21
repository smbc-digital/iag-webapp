namespace StockportWebappTests_Unit.Unit.Models;

public class SubItemTest
{
    [Fact]
    public void SetsNavigationLinkForATopic()
    {
        // Arrange
        SubItem subItem = new("slug", "title", "Teaser", "Icon", "topic", "image", new List<SubItem>(), EColourScheme.Teal);

        // Act & Assert
        Assert.Equal("/topic/slug", subItem.NavigationLink);
    }

    [Fact]
    public void SetsNavigationLinkForAnArticle()
    {
        // Arrange
        SubItem subItem = new("slug", "title", "Teaser", "Icon", "article", "image", new List<SubItem>(), EColourScheme.Teal);

        // Act & Assert
        Assert.Equal("/slug", subItem.NavigationLink);
    }

    [Fact]
    public void SetsNavigationLinkForAStartPage()
    {
        // Arrange
        SubItem subItem = new("slug", "title", "Teaser", "Icon", "start-page", "image", new List<SubItem>(), EColourScheme.Teal);

        // Act & Assert
        Assert.Equal("/start/slug", subItem.NavigationLink);
    }
}