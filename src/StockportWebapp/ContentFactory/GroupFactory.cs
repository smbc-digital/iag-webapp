using System.Text.RegularExpressions;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using Group = StockportWebapp.Models.Group;

namespace StockportWebapp.ContentFactory
{
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

            processedBody = Regex.Replace(processedBody, "<script", "<scri-pt", RegexOptions.IgnoreCase);
            processedBody = Regex.Replace(processedBody, "javascript", "javascri-pt", RegexOptions.IgnoreCase);

            var volunteering = new Volunteering()
            {
                Email = group.Email,
                VolunteeringText = group.VolunteeringText,
                VolunteeringNeeded = group.Volunteering,
                Url = $"groups/{group.Slug}"
            };

            var donations = new Donations()
            {
                Email = group.Email,
                GetDonations = group.Donations,
                Url = $"groups/{group.Slug}"
            };

            return new ProcessedGroup(group.Name, group.Slug, group.PhoneNumber, group.Email, group.Website, group.Twitter,
                group.Facebook, group.Address, processedBody, group.ImageUrl, group.ThumbnailImageUrl, group.CategoriesReference, 
                group.Breadcrumbs, group.MapPosition, group.Events, group.GroupAdministrators, group.DateHiddenFrom, group.DateHiddenTo, 
                group.Cost, group.CostText, group.AbilityLevel, group.Favourite, volunteering, group.Organisation, donations);
        }
    }
}