using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockportWebapp.Models;
using StockportWebapp.ViewModels;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedGroupHomepage : IProcessedContentType
    {
        public readonly List<GroupCategory> Categories = new List<GroupCategory>();
        public readonly PrimaryFilter PrimaryFilter;
        public readonly string BackgroundImage;
        public readonly string FeaturedGroupsHeading;
        public readonly List<Group> FeaturedGroups;
        public readonly GroupCategory FeaturedGroupsCategory;
        public readonly GroupSubCategory FeaturedGroupsSubCategory;
        public readonly List<Alert> Alerts;
        public readonly string Body;
        public readonly string SecondaryBody;

        public ProcessedGroupHomepage() { }

        public GenericFeaturedItemList GenericItemList
        {
            get
            {
                var result = new GenericFeaturedItemList();
                result.Items = new List<GenericFeaturedItem>();
                foreach (var cat in Categories)
                {
                    result.Items.Add(new GenericFeaturedItem { Icon = cat.Icon, Title = cat.Name, Url = $"/groups/results?category={cat.Slug}&order=Name+A-Z" });
                }

                result.ButtonText = "View more categories";

                return result;
            }
        }

        public ProcessedGroupHomepage(List<GroupCategory> categories, PrimaryFilter primaryFilter,
            string backgroundImage, string featuredGroupsHeading, List<Group> featuredGroups,
            GroupCategory featuredGroupsCategory, GroupSubCategory featuredGroupsSubCategory, List<Alert> alerts,
            string body, string secondaryBody)
        {
            Categories = categories;
            PrimaryFilter = primaryFilter;
            BackgroundImage = backgroundImage;
            FeaturedGroupsHeading = featuredGroupsHeading;
            FeaturedGroups = featuredGroups;
            FeaturedGroupsCategory = featuredGroupsCategory;
            FeaturedGroupsSubCategory = featuredGroupsSubCategory;
            Alerts = alerts;
            Body = body;
            SecondaryBody = secondaryBody;
        }
    }
}
