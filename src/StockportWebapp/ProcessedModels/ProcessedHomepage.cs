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

        public GenericFeaturedItemList GenericItemList
        {
            get
            {
                var result = new GenericFeaturedItemList();
                result.Items = new List<GenericFeaturedItem>();
                foreach (var topic in FeaturedTopics)
                {
                    var item = new GenericFeaturedItem
                    {
                        Icon = topic.Icon,
                        Title = topic.Title,
                        Url = $"/topic/{topic.Slug}",
                        SubItems = new List<GenericFeaturedItem>()
                    };

                    foreach (var subItem in topic.SubItems)
                    {
                        item.SubItems.Add(new GenericFeaturedItem { Title = subItem.Title, Url = subItem.NavigationLink, Icon = subItem.Icon });
                    }

                    result.Items.Add(item);
                }

                result.ButtonText = "View more services";
                result.ButtonCssClass = "green";

                return result;
            }
        }

        public ProcessedHomepage(IEnumerable<string> popularSearchTerms, string featuredTasksHeading, string featuredTasksSummary, IEnumerable<SubItem> featuredTasks, IEnumerable<Topic> featuredTopics, IEnumerable<Alert> alerts, IEnumerable<CarouselContent> carouselContents, string backgroundImage, IEnumerable<News> lastNews, string freeText, Group featuredGroup)
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
            FeaturedGroupItem = featuredGroup;
        }

        public News FeaturedNewsItem { get; set; }
        public Event FeaturedEventItem { get; set; }
        public Group FeaturedGroupItem { get; set; }

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
            FeaturedNewsItem = LatestNews?.First();
        }

        public void SetLatestEvents(List<Event> latestEvents)
        {
            LatestEvents = latestEvents;
            FeaturedEventItem = LatestEvents?.First();
        }
    }
}
