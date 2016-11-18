using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class NewsViewModel
    {
        public ProcessedNews NewsItem { get; }
        private List<News> LatestNewsItems { get; }

        public NewsViewModel(ProcessedNews newsItem, List<News> latestNewsItems)
        {
            NewsItem = newsItem;
            LatestNewsItems = latestNewsItems;
        }

        public List<News> GetLatestNews()
        {
            return LatestNewsItems;
        }
    }
}
