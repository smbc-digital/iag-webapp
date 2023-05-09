namespace StockportWebapp.ContentFactory;

public class OrganisationFactory
{
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly CookiesHelper cookiesHelper;

    public OrganisationFactory(MarkdownWrapper markdownWrapper, IHttpContextAccessor httpContextAccessor)
    {
        _markdownWrapper = markdownWrapper;
        cookiesHelper = new CookiesHelper(httpContextAccessor);
    }

    public virtual ProcessedOrganisation Build(Organisation organisation)
    {

        var body = _markdownWrapper.ConvertToHtml(organisation.AboutUs ?? "");

        var volunteering = new Volunteering
        {
            Email = organisation.Email,
            VolunteeringText = organisation.VolunteeringText,
            VolunteeringNeeded = organisation.Volunteering,
            Url = $"organisations/{organisation.Slug}",
            Type = "organisation"
        };

        var donations = new Donations()
        {
            Email = organisation.Email,
            GetDonations = organisation.Donations,
            Url = $"groups/{organisation.Slug}"
        };

        var groupsWithFavourites = cookiesHelper.PopulateCookies(organisation.Groups, "favourites");

        return new ProcessedOrganisation(organisation.Title, organisation.Slug, organisation.ImageUrl, body, organisation.Phone,
            organisation.Email, groupsWithFavourites, volunteering, donations);
    }
}
