namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class NewsViewModel(ProcessedNews newsItem, List<News> latestNewsItems)
{
    public ProcessedNews NewsItem { get; } = newsItem;
    private List<News> LatestNewsItems { get; } = latestNewsItems;

    public List<News> GetLatestNews() =>
        LatestNewsItems;
}