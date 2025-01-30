namespace StockportWebapp.Models.Groups;
[ExcludeFromCodeCoverage]
public class Group(string name,
                string slug,
                string metaDescription,
                string phoneNumber,
                string email,
                string website,
                string twitter,
                string facebook,
                string address,
                string description,
                string imageUrl,
                string thumbnailImageUrl,
                List<GroupCategory> categoriesReference,
                List<GroupSubCategory> subCategories,
                List<Crumb> breadcrumbs,
                MapPosition mapPosition,
                bool volunteering,
                List<Event> events,
                GroupAdministrators groupAdministrators,
                DateTime? dateHiddenFrom,
                DateTime? dateHiddenTo,
                string status,
                List<string> cost,
                string costText,
                string abilityLevel,
                bool favourite,
                string volunteeringText,
                Organisation organisation,
                List<Group> linkedGroups,
                bool donations,
                string accessibleTransportLink,
                List<GroupBranding> groupBranding,
                string additionalInformation,
                DateTime? dateLastModified,
                List<string> suitableFor,
                List<string> ageRange,
                string donationsText,
                string donationsUrl,
                IEnumerable<Alert> alerts,
                IEnumerable<Alert> alertsInline)
{
    public string Name { get; set; } = name;
    public string Slug { get; set; } = slug;
    public string MetaDescription { get; set; } = metaDescription;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string Email { get; set; } = email;
    public string Website { get; set; } = website;
    public string Twitter { get; set; } = twitter;
    public string Facebook { get; set; } = facebook;
    public string Address { get; set; } = address;
    public string Description { get; set; } = description;
    public string ImageUrl { get; set; } = imageUrl;
    public string ThumbnailImageUrl { get; set; } = thumbnailImageUrl;
    public List<GroupCategory> CategoriesReference { get; set; } = categoriesReference;
    public List<GroupSubCategory> SubCategories { get; set; } = subCategories;
    public List<Crumb> Breadcrumbs { get; set; } = breadcrumbs;
    public MapPosition MapPosition { get; set; } = mapPosition;
    public bool Volunteering { get; set; } = volunteering;
    public List<Event> Events { get; set; } = events;
    public GroupAdministrators GroupAdministrators { get; set; } = groupAdministrators;
    public DateTime? DateHiddenFrom { get; set; } = dateHiddenFrom;
    public DateTime? DateHiddenTo { get; set; } = dateHiddenTo;
    public string Status { get; set; } = status;
    public List<string> Cost { get; set; } = cost;
    public string CostText { get; set; } = costText;
    public string AbilityLevel { get; set; } = abilityLevel;
    public bool Favourite { get; set; } = favourite;
    public string VolunteeringText { get; set; } = volunteeringText;
    public Organisation Organisation { get; set; } = organisation;
    public List<Group> LinkedGroups { get; private set; } = linkedGroups;
    public bool Donations { get; set; } = donations;
    public string AccessibleTransportLink { get; set; } = accessibleTransportLink;
    public List<GroupBranding> GroupBranding { get; set; } = groupBranding;
    public string AdditionalInformation { get; set; } = additionalInformation;
    public DateTime? DateLastModified { get; set; } = dateLastModified;
    public List<string> SuitableFor { get; set; } = suitableFor;
    public List<string> AgeRange { get; set; } = ageRange;
    public string DonationsText { get; set; } = donationsText;
    public string DonationsUrl { get; set; } = donationsUrl;
    public IEnumerable<Alert> Alerts { get; } = alerts;
    public IEnumerable<Alert> AlertsInline { get; } = alertsInline;
}