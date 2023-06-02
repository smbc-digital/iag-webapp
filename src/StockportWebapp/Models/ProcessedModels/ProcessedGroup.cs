namespace StockportWebapp.Models.ProcessedModels;

public class ProcessedGroup : IProcessedContentType
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string MetaDescription { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string Twitter { get; set; }
    public string Facebook { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string ThumbnailImageUrl { get; set; }
    public List<GroupCategory> CategoriesReference { get; set; }
    public List<GroupSubCategory> SubCategories { get; set; }
    public List<Crumb> Breadcrumbs { get; set; }
    public List<Event> Events { get; set; }
    public GroupAdministrators GroupAdministrators { get; set; }
    public DateTime? DateHiddenFrom { get; set; }
    public DateTime? DateHiddenTo { get; set; }
    public DateTime? DateLastModified { get; set; }
    public List<string> Cost { get; set; }
    public string CostText { get; set; }
    public string AbilityLevel { get; set; }
    public bool Favourite { get; set; }
    public Volunteering Volunteering { get; set; }
    public Organisation Organisation { get; set; }
    public List<Group> LinkedGroups { get; set; }
    public Donations Donations { get; set; }
    public MapDetails MapDetails { get; set; }
    public string CurrentUrl { get; private set; }
    public string AdditionalInformation { get; set; }
    public string DonationsText { get; set; }
    public string DonationsUrl { get; set; }
    public List<GroupBranding> GroupBranding { get; set; }
    public IEnumerable<Alert> Alerts { get; set; }
    public string ProcessedBody { get; set; }
    public string ParsedAdditionalInformation { get; set; }

    public ProcessedGroup() { }

    public void SetCurrentUrl(string url)
    {
        CurrentUrl = url;
    }
}