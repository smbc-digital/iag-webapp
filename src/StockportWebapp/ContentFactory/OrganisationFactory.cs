using System.Collections.Generic;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class OrganisationFactory
    {
        private readonly MarkdownWrapper _markdownWrapper;

        public OrganisationFactory(MarkdownWrapper markdownWrapper)
        {
            _markdownWrapper = markdownWrapper;
        }

        public virtual ProcessedOrganisation Build(Organisation organisation)
        {
            
            var body = _markdownWrapper.ConvertToHtml(organisation.AboutUs ?? "");

            var volunteering = new Volunteering
            {
                Email = organisation.Email,
                VolunteeringText = organisation.VolunteeringText,
                VolunteeringNeeded = organisation.Volunteering,
                Url = $"organisations/{organisation.Slug}"
            };

            return new ProcessedOrganisation(organisation.Title, organisation.Slug, organisation.ImageUrl, body, organisation.Phone, 
                organisation.Email, organisation.Groups, volunteering);
        }
    }
}
