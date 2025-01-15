namespace StockportWebapp.ContentFactory;

public class GroupFactory(ITagParserContainer parser,
                        MarkdownWrapper markdownWrapper)
{
    private readonly ITagParserContainer _parser = parser;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

    public virtual ProcessedGroup Build(Group group)
    {
        string htmlBody = _markdownWrapper.ConvertToHtml(group.Description);
        string processedBody = _parser.ParseAll(htmlBody, group.Name);

        string additionalInformation = _markdownWrapper.ConvertToHtml(group.AdditionalInformation);
        string parsedAdditionalInformation = _parser.ParseAll(additionalInformation, group.Name);

        processedBody = Regex.Replace(processedBody, "<script", "<scri-pt", RegexOptions.IgnoreCase);
        processedBody = Regex.Replace(processedBody, "javascript", "javascri-pt", RegexOptions.IgnoreCase);

        Volunteering volunteering = new()
        {
            Email = group.Email,
            VolunteeringText = group.VolunteeringText,
            VolunteeringNeeded = group.Volunteering,
            Url = $"groups/{group.Slug}",
            Type = "group"
        };

        Donations donations = new()
        {

            GetDonations = group.Donations,
            Url = $"groups/{group.Slug}",
            Email = group.Email,
            DonationsText = group.DonationsText,
            DonationsUrl = group.DonationsUrl

        };

        MapDetails mapDetails = new()
        {
            MapPosition = group.MapPosition,
            AccessibleTransportLink = group.AccessibleTransportLink
        };

        return new ProcessedGroup
        {
            Name = group.Name,
            Description = processedBody,
            Slug = group.Slug,
            MetaDescription = group.MetaDescription,
            PhoneNumber = group.PhoneNumber,
            Email = group.Email,
            Website = group.Website,
            Twitter = group.Twitter,
            Facebook = group.Facebook,
            Address = group.Address,
            ImageUrl = group.ImageUrl,
            ThumbnailImageUrl = group.ThumbnailImageUrl,
            CategoriesReference = group.CategoriesReference,
            SubCategories = group.SubCategories,
            Breadcrumbs = group.Breadcrumbs,
            Events = group.Events,
            GroupAdministrators = group.GroupAdministrators,
            DateHiddenFrom = group.DateHiddenFrom,
            DateHiddenTo = group.DateHiddenTo,
            Cost = group.Cost,
            CostText = group.CostText,
            AbilityLevel = group.AbilityLevel,
            Favourite = group.Favourite,
            Volunteering = volunteering,
            Organisation = group.Organisation,
            LinkedGroups = group.LinkedGroups,
            Donations = donations,
            MapDetails = mapDetails,
            AdditionalInformation = parsedAdditionalInformation,
            DonationsText = group.DonationsText,
            DonationsUrl = group.DonationsUrl,
            DateLastModified = group.DateLastModified,
            GroupBranding = group.GroupBranding,
            Alerts = group.Alerts
        };
    }
}