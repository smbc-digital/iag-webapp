﻿namespace StockportWebapp.Models.ProcessedModels;
public class ProcessedGroupHomepage : IProcessedContentType
{
    public readonly string Title;
    public readonly string MetaDescription;
    public readonly List<GroupCategory> Categories = new();
    public readonly string BackgroundImage;
    public readonly string FeaturedGroupsHeading;
    public readonly List<Group> FeaturedGroups;
    public readonly GroupCategory FeaturedGroupsCategory;
    public readonly GroupSubCategory FeaturedGroupsSubCategory;
    public readonly List<Alert> Alerts;
    public readonly string BodyHeading;
    public readonly string Body;
    public readonly string SecondaryBodyHeading;
    public readonly string SecondaryBody;
    public readonly EventBanner EventBanner;

    public ProcessedGroupHomepage() { }

    public GenericFeaturedItemList GenericItemList => new()
    {
        Items = Categories.Select(cat => new GenericFeaturedItem(cat.Name, $"/groups/results?category={cat.Slug}&order=Name+A-Z", cat.Icon)).ToList(),
        ButtonText = "View more categories"
    };

    public ProcessedGroupHomepage(string title,
                                string metaDescription,
                                string backgroundImage,
                                string featuredGroupsHeading,
                                List<Group> featuredGroups,
                                GroupCategory featuredGroupsCategory,
                                GroupSubCategory featuredGroupsSubCategory,
                                List<Alert> alerts,
                                string bodyHeading,
                                string body,
                                string secondaryBodyHeading,
                                string secondaryBody,
                                EventBanner eventBanner)
    {
        Title = title;
        MetaDescription = metaDescription;
        BackgroundImage = backgroundImage;
        FeaturedGroupsHeading = featuredGroupsHeading;
        FeaturedGroups = featuredGroups;
        FeaturedGroupsCategory = featuredGroupsCategory;
        FeaturedGroupsSubCategory = featuredGroupsSubCategory;
        Alerts = alerts;
        BodyHeading = bodyHeading;
        Body = body;
        SecondaryBodyHeading = secondaryBodyHeading;
        SecondaryBody = secondaryBody;
        EventBanner = eventBanner;
    }
}