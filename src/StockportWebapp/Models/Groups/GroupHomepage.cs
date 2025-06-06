﻿namespace StockportWebapp.Models.Groups;
[ExcludeFromCodeCoverage]
public class GroupHomepage
{
    public string Title { get; set; }
    public string MetaDescription { get; set; }
    public string BackgroundImage { get; set; }
    public string FeaturedGroupsHeading { get; set; }
    public List<Group> FeaturedGroups { get; set; }
    public GroupCategory FeaturedGroupsCategory { get; set; }
    public GroupSubCategory FeaturedGroupsSubCategory { get; set; }
    public List<Alert> Alerts { get; set; }
    public string BodyHeading { get; set; }
    public string Body { get; set; }
    public string SecondaryBodyHeading { get; set; }
    public string SecondaryBody { get; set; }
    public EventBanner EventBanner { get; set; }

    public GroupHomepage() { }
}