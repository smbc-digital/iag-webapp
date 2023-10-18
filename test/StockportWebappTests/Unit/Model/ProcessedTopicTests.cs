namespace StockportWebappTests_Unit.Unit.Model;

public class ProcessedTopicTests
{
    private readonly ProcessedTopic processedTopic = new("topic name",
                                                            "healthy-living",
                                                            "summary",
                                                            "teaser",
                                                            "meta description",
                                                            "icon",
                                                            "backroundImage",
                                                            "image",
                                                            new List<SubItem> {
                                                                new("primary-items", "Primary Items", "Teaser", "Icon.ico", "Article", string.Empty, new List<SubItem>())
                                                            },
                                                            new List<SubItem> {
                                                                new("test-slug", "Featured Item", "Teaser", "Icon.ico", "Article", string.Empty, new List<SubItem>())
                                                            },
                                                            Enumerable.Empty<SubItem>(),
                                                            new List<Crumb>(),
                                                            new List<Alert> {
                                                                new("Test Alert", "Test", "Test Body", "Warning", new DateTime(2020, 1, 1), new DateTime(2040, 1, 1), "test-warning-alert", false, string.Empty),
                                                                new Alert("Test Alert", "Test", "Test Body", "Error", new DateTime(2020, 1, 1), new DateTime(2040, 1, 1), "test-error-alert", false, string.Empty),
                                                                new Alert("Test Alert", "Test", "Test Body", "Condolence", new DateTime(2020, 1, 1), new DateTime(2040, 1, 1), "test-condolence-alert", false, string.Empty),
                                                            },
                                                            true,
                                                            "email-alerts",
                                                            new EventBanner("title", "teaser", "icon", "link"),
                                                            "expandingLinkTitle",
                                                            Enumerable.Empty<ExpandingLinkBox>(),
                                                            "primary item title",
                                                            "title",
                                                            true,
                                                            null,
                                                            "event category",
                                                            new CallToActionBanner());

    [Fact]
    public void PrimaryItems_IsPopulated()
    {
        Assert.Single(processedTopic.PrimaryItems.Items);
    }
}