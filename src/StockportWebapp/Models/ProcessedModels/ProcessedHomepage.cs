﻿namespace StockportWebapp.Models.ProcessedModels
{
    public class ProcessedHomepage : IProcessedContentType
    {
        public readonly IEnumerable<string> PopularSearchTerms;
        public readonly string FeaturedTasksHeading;
        public readonly string FeaturedTasksSummary;
        public readonly IEnumerable<SubItem> FeaturedTasks;
        public readonly IEnumerable<SubItem> FeaturedTopics;
        public readonly IEnumerable<Alert> Alerts;
        public readonly IEnumerable<Alert> CondolenceAlerts;
        public readonly IEnumerable<CarouselContent> CarouselContents;
        public readonly string BackgroundImage;
        public readonly string ForegroundImage;
        public readonly string ForegroundImageLocation;
        public readonly string ForegroundImageLink;
        public readonly string ForegroundImageAlt;
        public readonly string FreeText;
        public readonly Group FeaturedGroupItem;
        public readonly string EventCategory;
        public readonly string MetaDescription;
        public readonly CarouselContent CampaignBanner;
        public readonly CallToActionBanner CallToAction;
        public readonly IEnumerable<SpotlightOnBanner> SpotlightOnBanner;

        public ProcessedHomepage(IEnumerable<string> popularSearchTerms, string featuredTasksHeading, string featuredTasksSummary, IEnumerable<SubItem> featuredTasks, IEnumerable<SubItem> featuredTopics, IEnumerable<Alert> alerts, IEnumerable<CarouselContent> carouselContents, string backgroundImage, string foregroundImage, string foregroundImageLocation, string foregroundImageLink, string foregroundImageAlt, IEnumerable<News> lastNews, string freeText, Group featuredGroup, string eventCategory, string metaDescription, CarouselContent campaignBanner, CallToActionBanner callToAction, IEnumerable<SpotlightOnBanner> spotlightOnBanner)
        {
            PopularSearchTerms = popularSearchTerms;
            FeaturedTasksHeading = featuredTasksHeading;
            FeaturedTasksSummary = featuredTasksSummary;
            FeaturedTasks = featuredTasks;
            FeaturedTopics = featuredTopics;
            Alerts = alerts.Where(_ => !_.Severity.Equals(Severity.Condolence));
            CondolenceAlerts = alerts.Where(_ => _.Severity.Equals(Severity.Condolence));
            CarouselContents = carouselContents;
            BackgroundImage = backgroundImage;
            ForegroundImage = foregroundImage;
            ForegroundImageLocation = foregroundImageLocation;
            ForegroundImageLink = foregroundImageLink;
            ForegroundImageAlt = foregroundImageAlt;
            FreeText = freeText;
            FeaturedGroupItem = featuredGroup;
            EventCategory = eventCategory;
            MetaDescription = metaDescription;
            CampaignBanner = campaignBanner;
            CallToAction = callToAction;
            SpotlightOnBanner = spotlightOnBanner;
        }

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

        public NavCardList Services
        {
            get
            {
                var result = new NavCardList();
                result.Items = new List<NavCard>();
                foreach (var topic in FeaturedTopics)
                {
                    var item = new NavCard
                    {
                        Title = topic.Title,
                        Url = topic.NavigationLink,
                        Teaser = topic.Teaser
                    };

                    result.Items.Add(item);
                }

                result.ButtonText = "View more services";

                return result;
            }
        }
    }
}
