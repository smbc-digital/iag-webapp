namespace StockportWebappTests_Unit.Unit.Models;

public class EventHomepageTests
{
    [Fact]
    public void GenericItemList_ShouldReturnGenericFeaturedItemList()
    {
        // Arrange
        EventHomepage eventHomepage = new(new List<Alert>());
        eventHomepage.Categories = new List<EventCategory>
        {
            new()
            {
                Name = "name",
                Slug = "slug",
                Icon = "icon"
            }
        };

        // Act
        GenericFeaturedItemList result = eventHomepage.GenericItemList;

        // Assert
        Assert.Single(result.Items);
    }
}