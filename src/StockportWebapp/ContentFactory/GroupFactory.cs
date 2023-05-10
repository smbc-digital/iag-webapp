namespace StockportWebapp.ContentFactory;

public class GroupFactory
{
    private readonly ISimpleTagParserContainer _parser;
    private readonly MarkdownWrapper _markdownWrapper;

    public GroupFactory(ISimpleTagParserContainer parser, MarkdownWrapper markdownWrapper)
    {
        _parser = parser;
        _markdownWrapper = markdownWrapper;
    }

    public virtual ProcessedGroup Build(Group group)
    {
        var htmlBody = _markdownWrapper.ConvertToHtml(group.Description);
        var processedBody = _parser.ParseAll(htmlBody, group.Name);

        var additionalInformation = _markdownWrapper.ConvertToHtml(group.AdditionalInformation);
        var parsedAdditionalInformation = _parser.ParseAll(additionalInformation, group.Name);

        processedBody = Regex.Replace(processedBody, "<script", "<scri-pt", RegexOptions.IgnoreCase);
        processedBody = Regex.Replace(processedBody, "javascript", "javascri-pt", RegexOptions.IgnoreCase);

        var volunteering = new Volunteering()
        {
            Email = group.Email,
            VolunteeringText = group.VolunteeringText,
            VolunteeringNeeded = group.Volunteering,
            Url = $"groups/{group.Slug}",
            Type = "group"
        };

        var donations = new Donations()
        {

            GetDonations = group.Donations,
            Url = $"groups/{group.Slug}",
            Email = group.Email,
            DonationsText = group.DonationsText,
            DonationsUrl = group.DonationsUrl

        };
        var mapDetails = new MapDetails()
        {
            MapPosition = group.MapPosition,
            AccessibleTransportLink = group.AccessibleTransportLink
        };

        return new ProcessedGroup
        {
            Name = group.Name,
            ProcessedBody = processedBody,
            Description = group.Description,
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