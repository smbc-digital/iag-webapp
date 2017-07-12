using System.Collections.Generic;
using System.Linq;

namespace StockportWebapp.Models
{
    public class Homepage
    {
        public IEnumerable<string> PopularSearchTerms { get; }
        public string FeaturedTasksHeading { get; }
        public string FeaturedTasksSummary { get; }
        public IEnumerable<SubItem> FeaturedTasks;
        public IEnumerable<Topic> FeaturedTopics { get; }
        public IEnumerable<Alert> Alerts { get; }
        public IEnumerable<CarouselContent> CarouselContents { get; }
        public string BackgroundImage { get; }
        public IEnumerable<News> LastNews { get; set; }
        public string FreeText { get; }
        public Group FeaturedGroup { get; }

        public Homepage(IEnumerable<string> popularSearchTerms, string featuredTasksHeading, string featuredTasksSummary, IEnumerable<SubItem> featuredTasks, IEnumerable<Topic> featuredTopics, IEnumerable<Alert> alerts, IEnumerable<CarouselContent> carouselContents, string backgroundImage, string freeText, Group featuredGroup)
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
            FeaturedGroup = featuredGroup;
        }
    }

    public class NullHomepage : Homepage
    {
        public NullHomepage() : base(Enumerable.Empty<string>(), string.Empty, string.Empty, new List<SubItem>(), new List<Topic>(), new List<Alert>(), new List<CarouselContent>(), "", string.Empty, null) { }
    }
}
