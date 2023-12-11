using StockportGovUK.NetStandard.Gateways.Models.RevsAndBens;
using System.Linq;

namespace StockportWebappTests_Unit.Unit.Extensions;

public class FeatureCardViewModelMapperExtensionsTests
{
    [Fact]
    public void ShouldMapEventToFeaturedItemCorrectly()
    {
        var featuredEvent = new Event {
                Title = "TestEvent",
                Teaser = "EventTeaser",
                EventDate = DateTime.Today,
                StartTime = "9am",
                ImageUrl = "/image.jpg",
                Slug = "MyTestEvent"
        };

        var result = featuredEvent.MapToFeatureCard();
        
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
        var featuredNews = new News(
            "TestNewsItem", 
            "TestNewItem", 
            "Test News Teaser", 
            "Test",
            "/News.jpg",
            "/NewsThumb.jpg",
            "Test News Body",
            new List<Crumb>(),
            DateTime.Today.AddDays(-2),
            DateTime.Today.AddDays(2),
            DateTime.Today.AddDays(-1),
            new List<Alert>(),
            new List<string>(),
            new List<StockportWebapp.Models.Document>(),
            new List<Profile>());


        var result = featuredNews.MapToFeatureCard();
        
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
