using Xunit;

namespace StockportWebappTests_Unit.Unit.Model;

public class ProcessedHomepageTests
{
    private readonly ProcessedHomepage processedHomepage = new ProcessedHomepage(Enumerable.Empty<string>(),
                                                                                    "Featured Tasks",
                                                                                    "Featured tasks summary",
                                                                                    Enumerable.Empty<SubItem>(),
                                                                                    new List<SubItem> {
                                                                                        new SubItem("test-slug", "Featured Item", "Teaser", "Icon.ico", "Article", string.Empty, new List<SubItem>())
                                                                                    },
                                                                                    new List<Alert> {
                                                                                        new Alert("Test Alert", "Test", "Test Body", "Warning", new DateTime(2020, 1, 1), new DateTime(2040, 1, 1), "test-warning-alert", false, string.Empty),
                                                                                        new Alert("Test Alert", "Test", "Test Body", "Error", new DateTime(2020, 1, 1), new DateTime(2040, 1, 1), "test-error-alert", false, string.Empty),
                                                                                        new Alert("Test Alert", "Test", "Test Body", "Condolence", new DateTime(2020, 1, 1), new DateTime(2040, 1, 1), "test-condolence-alert", false, string.Empty),
                                                                                    },
                                                                                    Enumerable.Empty<CarouselContent>(),
                                                                                    string.Empty,
                                                                                    Enumerable.Empty<News>(),
                                                                                    "Free text",
                                                                                    null,
                                                                                    "Event category",
                                                                                    "meta description",
                                                                                    null);

    [Fact]
    public void Should_Seperate_CondolenceAlerts_From_Other_Alerts()
    {
        Assert.Single(processedHomepage.CondolenceAlerts);
        Assert.Equal(2, processedHomepage.Alerts.Count());
    }

    [Fact]
    public void ServicesList_IsPopulated()
    {
        Assert.Single(processedHomepage.Services.Items);
    }
}
