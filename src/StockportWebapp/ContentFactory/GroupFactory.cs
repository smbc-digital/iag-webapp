using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

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

            return new ProcessedGroup(group.Name, group.Slug, group.PhoneNumber, group.Email, group.Website, group.Twitter,
                group.Facebook, group.Address, processedBody, group.ImageUrl, group.ThumbnailImageUrl, group.CategoriesReference, group.Breadcrumbs, group.MapPosition, group.Volunteering, group.Events, group.GroupAdministrators);
        }
    }
}