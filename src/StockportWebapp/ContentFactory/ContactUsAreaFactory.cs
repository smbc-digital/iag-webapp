using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class ContactUsAreaFactory
    {
        private readonly ISimpleTagParserContainer _tagParserContainer;
        private readonly MarkdownWrapper _markdownWrapper;

        public ContactUsAreaFactory(ISimpleTagParserContainer tagParserContainer, MarkdownWrapper markdownWrapper)
        {
            _tagParserContainer = tagParserContainer;
            _markdownWrapper = markdownWrapper;
        }

        public virtual ProcessedContactUsArea Build(ContactUsArea contactUsArea)
        {
            var body = _tagParserContainer.ParseAll(contactUsArea.Body);
            contactUsArea.Body = _markdownWrapper.ConvertToHtml(body ?? string.Empty);

            return new ProcessedContactUsArea(
                contactUsArea.Title,
                contactUsArea.Slug,
                contactUsArea.Teaser,
                contactUsArea.Body,
                contactUsArea.Breadcrumbs,
                contactUsArea.PrimaryItems,
                contactUsArea.Alerts
            );
        }
    }
}
