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
            return new ProcessedContactUsArea(
                contactUsArea.Title,
                contactUsArea.Slug,
                contactUsArea.Breadcrumbs,
                contactUsArea.PrimaryItems,
                contactUsArea.Alerts
            );
        }
    }
}
