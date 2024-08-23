namespace StockportWebappTests_Unit.Unit.Model;

public class SubItemTest
{
    [Fact]
    public void SetsNavigationLinkForATopic()
    {
        // Arrange
        SubItem subItem = new("slug", "title", "Teaser", "Icon", "topic", "contentType", "image", 0, "body text", new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);

        // Act & Assert
        Assert.Equal("/topic/slug", subItem.NavigationLink);
    }

    [Fact]
    public void SetsNavigationLinkForAnArticle()
    {
        // Arrange
        SubItem subItem = new("slug", "title", "Teaser", "Icon", "article", "contentType", "image", 0, "body text", new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);

        // Act & Assert
        Assert.Equal("/slug", subItem.NavigationLink);
    }

    [Fact]
    public void SetsNavigationLinkForAStartPage()
    {
        // Arrange
        SubItem subItem = new("slug", "title", "Teaser", "Icon", "start-page", "contentType", "image", 0, "body text", new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);

        // Act & Assert
        Assert.Equal("/start/slug", subItem.NavigationLink);
    }
}