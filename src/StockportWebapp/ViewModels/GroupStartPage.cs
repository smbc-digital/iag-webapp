namespace StockportWebapp.ViewModels;

[Obsolete("Groups is being replaced by directories/directory entries")]
[ExcludeFromCodeCoverage(Justification = "Obsolete")]
public class GroupStartPage
{
    public List<GroupCategory> Categories = new List<GroupCategory>();
    public PrimaryFilter PrimaryFilter { set; get; }
    public string BackgroundImage { set; get; }
    public string FeaturedGroupsHeading { get; set; }
    public List<Group> FeaturedGroups { get; set; }
    public GroupCategory FeaturedGroupsCategory { get; set; }
    public GroupSubCategory FeaturedGroupsSubCategory { get; set; }
    public List<Alert> Alerts { get; set; }
    public string BodyHeading { get; set; }
    public string Body { get; set; }
    public string SecondaryBodyHeading { get; set; }
    public string SecondaryBody { get; set; }
    public string MetaDescription { get; set; }
    public EventBanner EventBanner { get; set; }

    public GroupStartPage() { }

    public GenericFeaturedItemList GenericItemList => new()
    {
        Items = Categories.Select(cat => new GenericFeaturedItem(cat.Name, $"/groups/results?category={cat.Slug}&order=Name+A-Z", cat.Icon)).ToList(),
        ButtonText = string.Empty,
        HideButton = true
    };
}
