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

        return new ProcessedGroup(group.Name, group.Slug, group.MetaDescription, group.PhoneNumber, group.Email, group.Website, group.Twitter,
            group.Facebook, group.Address, processedBody, group.ImageUrl, group.ThumbnailImageUrl, group.CategoriesReference, group.SubCategories,
            group.Breadcrumbs, group.Events, group.GroupAdministrators, group.DateHiddenFrom, group.DateHiddenTo,
            group.Cost, group.CostText, group.AbilityLevel, group.Favourite, volunteering, group.Organisation,
            group.LinkedGroups, donations, mapDetails, parsedAdditionalInformation, group.DonationsText, group.DonationsUrl,
            group.DateLastModified, group.GroupBranding, group.Alerts);
    }
}