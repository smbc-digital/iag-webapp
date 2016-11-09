using System.Collections.Generic;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.Models
{
    public class NewsViewModel
    {
        public ProcessedNews NewsItem { get; }
        public FeatureToggles FeatureToggles { get; }
        private List<News> LatestNewsItems { get; }

        public NewsViewModel(ProcessedNews newsItem, List<News> latestNewsItems, FeatureToggles featureToggles)
        {
            NewsItem = newsItem;
            LatestNewsItems = latestNewsItems;
            FeatureToggles = featureToggles;
        }

        public List<News> GetLatestNews()
        {
            return LatestNewsItems;
        }
    }
}
