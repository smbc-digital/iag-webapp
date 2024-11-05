using Org.BouncyCastle.Crypto.Modes;

namespace StockportWebappTests_Unit.Unit.Models.ProcessedModels;

public class ProcessedTopicTests
{
    private readonly ProcessedTopic processedTopic = new("topic name",
                                                            "healthy-living",
                                                            "summary",
                                                            "teaser",
                                                            "meta description",
                                                            "icon",
                                                            "backgroundImage",
                                                            "image",
                                                            new List<SubItem> {
                                                                new("featured-items", "Featured Items", "Teaser", "teaser image", "Icon.ico", string.Empty, string.Empty, new List<SubItem>(), EColourScheme.Teal)
                                                            },
                                                            new List<SubItem> {
                                                                new("primary-items", "Primary Items", "Teaser", "teaser image", "Icon.ico", string.Empty, string.Empty, new List<SubItem>(), EColourScheme.Teal)
                                                            },
                                                            new List<SubItem> {
                                                                new("test-slug", "Featured Item", "Teaser", "teaser image", "Icon.ico", string.Empty, string.Empty, new List<SubItem>(), EColourScheme.Teal)
                                                            },
                                                            new List<Crumb>(),
                                                            new List<Alert> {
                                                                new("Test Alert", "Test", "Test Body", "Warning", new DateTime(2020, 1, 1), new DateTime(2040, 1, 1), "test-warning-alert", false, string.Empty),
                                                                new("Test Alert", "Test", "Test Body", "Error", new DateTime(2020, 1, 1), new DateTime(2040, 1, 1), "test-error-alert", false, string.Empty),
                                                                new("Test Alert", "Test", "Test Body", "Condolence", new DateTime(2020, 1, 1), new DateTime(2040, 1, 1), "test-condolence-alert", false, string.Empty),
                                                            },
                                                            true,
                                                            "email-alerts",
                                                            new EventBanner("title", "teaser", "link", "icon"),
                                                            new EventCalendarBanner()
                                                            {
                                                                Title = "title",
                                                                Teaser = "teaser",
                                                                Link = "link",
                                                                Icon = "icon",
                                                                Colour = EColourScheme.Teal
                                                            },
                                                            "title",
                                                            true,
                                                            null,
                                                            "event category",
                                                            new CallToActionBanner(),
                                                            null,
                                                            string.Empty);

    [Fact]
    public void PrimaryItems_IsPopulated()
    {
        // Act & Assert
        Assert.Single(processedTopic.PrimaryItems.Items);
    }
}