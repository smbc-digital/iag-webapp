using System.Collections.Generic;
using System.Linq;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedHomepage : IProcessedContentType
    {
        public IEnumerable<string> PopularSearchTerms { get; }
        public readonly string FeaturedTasksHeading;
        public readonly string FeaturedTasksSummary;
        public readonly IEnumerable<SubItem> FeaturedTasks;
        public readonly IEnumerable<Topic> FeaturedTopics;
        public readonly IEnumerable<Alert> Alerts;
        public readonly IEnumerable<CarouselContent> CarouselContents;
        public readonly string BackgroundImage;  
        public readonly string FreeText;
        private IEnumerable<News> LatestNews { get; set; }
        private IEnumerable<Event> LatestEvents { get; set; }

        public ProcessedHomepage(IEnumerable<string> popularSearchTerms, string featuredTasksHeading, string featuredTasksSummary, IEnumerable<SubItem> featuredTasks, IEnumerable<Topic> featuredTopics, IEnumerable<Alert> alerts, IEnumerable<CarouselContent> carouselContents, string backgroundImage, IEnumerable<News> lastNews, string freeText)
        {
            PopularSearchTerms = popularSearchTerms;
            FeaturedTasksHeading = featuredTasksHeading;
            FeaturedTasksSummary = featuredTasksSummary;
            FeaturedTasks = featuredTasks;
            FeaturedTopics = featuredTopics;
            Alerts = alerts;
            CarouselContents = carouselContents;
            BackgroundImage = backgroundImage;
            LatestNews = lastNews;
            FreeText = freeText;
        }

        public List<News> GetLatestNews()
        {
            return LatestNews == null ? new List<News>() : LatestNews.ToList();
        }

        public List<Event> GetLatestEvents()
        {
            return LatestEvents == null ? new List<Event>() : LatestEvents.ToList();
        }

        public void SetLatestNews(List<News> latestNews)
        {
            LatestNews = latestNews;
        }

        public void SetLatestEvents(List<Event> latestEvents)
        {
            LatestEvents = latestEvents;
        }
    }
}
