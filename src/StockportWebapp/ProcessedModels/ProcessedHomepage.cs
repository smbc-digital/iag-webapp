using System.Collections.Generic;
using System.Linq;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedHomepage : IProcessedContentType
    {
        public readonly IEnumerable<string> PopularSearchTerms;
        public readonly string FeaturedTasksHeading;
        public readonly string FeaturedTasksSummary;
        public readonly IEnumerable<SubItem> FeaturedTasks;
        public readonly IEnumerable<SubItem> FeaturedTopics;
        public readonly IEnumerable<Alert> Alerts;
        public readonly IEnumerable<CarouselContent> CarouselContents;
        public readonly string BackgroundImage;  
        public readonly string FreeText;
        public readonly Group FeaturedGroupItem;
        public readonly string EventCategory;

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
                        Url = topic.NavigationLink,
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

        public ProcessedHomepage(IEnumerable<string> popularSearchTerms, string featuredTasksHeading, string featuredTasksSummary, IEnumerable<SubItem> featuredTasks, IEnumerable<SubItem> featuredTopics, IEnumerable<Alert> alerts, IEnumerable<CarouselContent> carouselContents, string backgroundImage, IEnumerable<News> lastNews, string freeText, Group featuredGroup, string eventCategory)
        {
            PopularSearchTerms = popularSearchTerms;
            FeaturedTasksHeading = featuredTasksHeading;
            FeaturedTasksSummary = featuredTasksSummary;
            FeaturedTasks = featuredTasks;
            FeaturedTopics = featuredTopics;
            Alerts = alerts;
            CarouselContents = carouselContents;
            BackgroundImage = backgroundImage;
            FreeText = freeText;
            FeaturedGroupItem = featuredGroup;
            EventCategory = eventCategory;
        }
    }
}
