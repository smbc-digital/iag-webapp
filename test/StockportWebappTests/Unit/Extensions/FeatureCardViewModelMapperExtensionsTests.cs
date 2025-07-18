using StockportGovUK.NetStandard.Gateways.Models.RevsAndBens;
using System.Linq;

namespace StockportWebappTests_Unit.Unit.Extensions;

public class FeatureCardViewModelMapperExtensionsTests
{
    [Fact]
    public void ShouldMapEventToFeaturedItemCorrectly()
    {
        // Arrange
        Event featuredEvent = new() {
                Title = "TestEvent",
                Teaser = "EventTeaser",
                EventDate = DateTime.Today,
                StartTime = "9am",
                ImageUrl = "/image.jpg",
                Slug = "MyTestEvent"
        };

        // Act
        FeatureCardViewModel result = featuredEvent.MapToFeatureCard();
        
        // Assert
        Assert.IsType<FeatureCardViewModel>(result);
        Assert.Equal(featuredEvent.Title, result.Title);
        Assert.Equal(featuredEvent.Teaser, result.Teaser);
        Assert.Equal(featuredEvent.EventDate, result.Date);
        Assert.Equal(featuredEvent.StartTime, result.StartTime);
        Assert.Equal(featuredEvent.ImageUrl, result.Image);
        Assert.Equal("View more events", result.ButtonText);
        Assert.Equal($"/events/{featuredEvent.Slug}", result.Slug);
        Assert.Equal("Events", result.ButtonTargetController);
        Assert.Equal("Upcoming event", result.HeaderText);                
    }

        [Fact]
    public void ShouldMapNewsToFeaturedItemCorrectly()
    {
        // Arrange
        News featuredNews = new(
            "TestNewsItem", 
            "TestNewItem", 
            "Test News Teaser", 
            "Test",
            "heroImage.png",
            "/News.jpg",
            "/NewsThumb.jpg",
            "hero image caption",
            "Test News Body",
            new List<Crumb>(),
            DateTime.Today.AddDays(-2),
            "test",
            DateTime.Today.AddDays(2),
            DateTime.Today.AddDays(-1),
            new List<Alert>(),
            new List<string>(),
            new List<StockportWebapp.Models.Document>(),
            new List<Profile>(),
            new List<InlineQuote>(),
            null,
            "in partnership with",
            new List<TrustedLogo>(),
            null,
            "eventsByTagOrCategory",
            new List<Event>()
            );

        // Act
        FeatureCardViewModel result = featuredNews.MapToFeatureCard();
        
        // Assert
        Assert.IsType<FeatureCardViewModel>(result);
        Assert.Equal(featuredNews.Title, result.Title);
        Assert.Equal(featuredNews.Teaser, result.Teaser);
        Assert.Equal(featuredNews.UpdatedAt, result.Date);
        Assert.Equal(string.Empty, result.StartTime);
        Assert.Equal(featuredNews.Image, result.Image);
        Assert.Equal("View more news", result.ButtonText);
        Assert.Equal($"/news/{featuredNews.Slug}", result.Slug);
        Assert.Equal("Comms", result.ButtonTargetController);
        Assert.Equal("Latest news", result.HeaderText);      
    }
}