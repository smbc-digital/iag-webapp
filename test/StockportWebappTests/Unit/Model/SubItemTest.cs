namespace StockportWebappTests_Unit.Unit.Model;

public class SubItemTest
{
    [Fact]
    public void SetsNavigationLinkForATopic()
    {
        // Arrange
        SubItem subItem = new("slug", "title", "Teaser", "Icon", "topic", "contentType", "image", string.Empty, "body text", new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal);

        // Act & Assert
        Assert.Equal("/topic/slug", subItem.NavigationLink);
    }

    [Fact]
    public void SetsNavigationLinkForAnArticle()
    {
        // Arrange
        SubItem subItem = new("slug", "title", "Teaser", "Icon", "article", "contentType", "image", string.Empty, "body text", new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal);

        // Act & Assert
        Assert.Equal("/slug", subItem.NavigationLink);
    }

    [Fact]
    public void SetsNavigationLinkForAStartPage()
    {
        // Arrange
        SubItem subItem = new("slug", "title", "Teaser", "Icon", "start-page", "contentType", "image", string.Empty, "body text", new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal);

        // Act & Assert
        Assert.Equal("/start/slug", subItem.NavigationLink);
    }
}