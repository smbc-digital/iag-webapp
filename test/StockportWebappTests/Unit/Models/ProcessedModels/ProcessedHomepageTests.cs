namespace StockportWebappTests_Unit.Unit.Models.ProcessedModels;

public class ProcessedHomepageTests
{
    private readonly ProcessedHomepage processedHomepage = new("Title",
                                                            "Featured Tasks",
                                                            "Featured tasks summary",
                                                            Enumerable.Empty<SubItem>(),
                                                            new List<SubItem> {
                                                                new("test-slug", "Featured Item", "Teaser", "teaser image", "Icon.ico", "Article", string.Empty, new List<SubItem>(), EColourScheme.Teal)
                                                            },
                                                            new List<Alert> {
                                                                new("Test Alert", "Test Body", "Warning", new DateTime(2020, 1, 1), new DateTime(2040, 1, 1), "test-warning-alert", false, string.Empty),
                                                                new("Test Alert", "Test Body", "Error", new DateTime(2020, 1, 1), new DateTime(2040, 1, 1), "test-error-alert", false, string.Empty),
                                                                new("Test Alert", "Test Body", "Condolence", new DateTime(2020, 1, 1), new DateTime(2040, 1, 1), "test-condolence-alert", false, string.Empty),
                                                            },
                                                            Enumerable.Empty<CarouselContent>(),
                                                            string.Empty,
                                                            string.Empty,
                                                            string.Empty,
                                                            string.Empty,
                                                            string.Empty,
                                                            Enumerable.Empty<News>(),
                                                            "Free text",
                                                            "Event category",
                                                            "meta description",
                                                            null,
                                                            new CallToActionBanner(),
                                                            new CallToActionBanner(),
                                                            Enumerable.Empty<SpotlightOnBanner>(),
                                                            "image overlay text");

    [Fact]
    public void Should_Separate_CondolenceAlerts_From_Other_Alerts()
    {
        // Act & Assert
        Assert.Single(processedHomepage.CondolenceAlerts);
        Assert.Equal(2, processedHomepage.Alerts.Count());
    }

    [Fact]
    public void ServicesList_IsPopulated()
    {
        // Act & Assert
        Assert.Single(processedHomepage.Services.Items);
    }
}