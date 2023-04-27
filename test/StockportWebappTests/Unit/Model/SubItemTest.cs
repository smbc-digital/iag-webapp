namespace StockportWebappTests_Unit.Unit.Model;

public class SubItemTest
{
    [Fact]
    public void SetsNavigationLinkForATopic()
    {
        var subItem = new SubItem("slug", "title", "Teaser", "Icon", "topic", "image", new List<SubItem>());

        Assert.Equal("/topic/slug", subItem.NavigationLink);
    }

    [Fact]
    public void SetsNavigationLinkForAnArticle()
    {
        var subItem = new SubItem("slug", "title", "Teaser", "Icon", "article", "image", new List<SubItem>());

        Assert.Equal("/slug", subItem.NavigationLink);
    }

    [Fact]
    public void SetsNavigationLinkForAStartPage()
    {
        var subItem = new SubItem("slug", "title", "Teaser", "Icon", "start-page", "image", new List<SubItem>());

        Assert.Equal("/start/slug", subItem.NavigationLink);
    }

}