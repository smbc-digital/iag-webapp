namespace StockportWebapp.ContentFactory;

[ExcludeFromCodeCoverage]
public class OrganisationFactory(MarkdownWrapper markdownWrapper,
                                IHttpContextAccessor httpContextAccessor)
{
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;
    private readonly CookiesHelper _cookiesHelper = new CookiesHelper(httpContextAccessor);

    public virtual ProcessedOrganisation Build(Organisation organisation)
    {
        string body = _markdownWrapper.ConvertToHtml(organisation.AboutUs ?? "");

        Volunteering volunteering = new()
        {
            Email = organisation.Email,
            VolunteeringText = organisation.VolunteeringText,
            VolunteeringNeeded = organisation.Volunteering,
            Url = $"organisations/{organisation.Slug}",
            Type = "organisation"
        };

        Donations donations = new()
        {
            Email = organisation.Email,
            GetDonations = organisation.Donations,
            Url = $"groups/{organisation.Slug}"
        };

        List<Group> groupsWithFavourites = _cookiesHelper.PopulateCookies(organisation.Groups, "favourites");

        return new ProcessedOrganisation(organisation.Title,
            organisation.Slug,
            organisation.ImageUrl,
            body,
            organisation.Phone,
            organisation.Email,
            groupsWithFavourites,
            volunteering,
            donations);
    }
}