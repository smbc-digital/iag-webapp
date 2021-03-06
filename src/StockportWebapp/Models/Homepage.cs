﻿using System.Collections.Generic;
using System.Linq;

namespace StockportWebapp.Models
{
    public class Homepage
    {
        public IEnumerable<string> PopularSearchTerms { get; }
        public string FeaturedTasksHeading { get; }
        public string FeaturedTasksSummary { get; }
        public IEnumerable<SubItem> FeaturedTasks;
        public IEnumerable<SubItem> FeaturedTopics { get; }
        public IEnumerable<Alert> Alerts { get; }
        public IEnumerable<CarouselContent> CarouselContents { get; }
        public string BackgroundImage { get; }
        public IEnumerable<News> LastNews { get; set; }
        public string FreeText { get; }
        public Group FeaturedGroup { get; }
        public string EventCategory { get; }
        public string MetaDescription { get; set; }

        public Homepage(IEnumerable<string> popularSearchTerms, string featuredTasksHeading, string featuredTasksSummary, IEnumerable<SubItem> featuredTasks, IEnumerable<SubItem> featuredTopics, IEnumerable<Alert> alerts, IEnumerable<CarouselContent> carouselContents, string backgroundImage, string freeText, Group featuredGroup, string eventCategory, string metaDescription)
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
            EventCategory = eventCategory;
            MetaDescription = metaDescription;
        }
    }

    public class NullHomepage : Homepage
    {
        public NullHomepage() : base(Enumerable.Empty<string>(), string.Empty, string.Empty, new List<SubItem>(), new List<SubItem>(), new List<Alert>(), new List<CarouselContent>(), "", string.Empty, null, string.Empty, string.Empty) { }
    }
}
