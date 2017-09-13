using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class OrganisationFactory
    {
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly FavouritesHelper favouritesHelper;

        public OrganisationFactory(MarkdownWrapper markdownWrapper, IHttpContextAccessor httpContextAccessor)
        {
            _markdownWrapper = markdownWrapper;
            favouritesHelper = new FavouritesHelper(httpContextAccessor);
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

            var groupsWithFavourites = favouritesHelper.PopulateFavourites(organisation.Groups);

            return new ProcessedOrganisation(organisation.Title, organisation.Slug, organisation.ImageUrl, body, organisation.Phone, 
                organisation.Email, groupsWithFavourites, volunteering, donations);
        }
    }
}
