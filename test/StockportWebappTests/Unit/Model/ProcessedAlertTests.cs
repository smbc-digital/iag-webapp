namespace StockportWebappTests_Unit.Unit.Model;

public class ProcessedHomepageTests
{
    private readonly ProcessedHomepage processedHomepage = new ProcessedHomepage(Enumerable.Empty<string>(), "Featured Tasks", "Featured tasks summary", Enumerable.Empty<SubItem>(), Enumerable.Empty<SubItem>(), Enumerable.Empty<Alert>(), Enumerable.Empty<CarouselContent>(), string.Empty, Enumerable.Empty<News>(), "Free text", null, "Event category", "meta description", null);

    [Fact (Skip = "Not Implemented")]
    public void Should_Seperate_CondolenceAlerts_From_Other_Alerts()
    {
        
    }
}
