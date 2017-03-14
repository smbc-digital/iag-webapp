using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class PaymentFactory
    {
        private readonly ISimpleTagParserContainer _simpleTagParserContainer;
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly IDynamicTagParser<Document> _documentTagParser;

        public PaymentFactory(ISimpleTagParserContainer simpleTagParserContainer, MarkdownWrapper markdownWrapper, IDynamicTagParser<Document> documentTagParser)
        {
            _simpleTagParserContainer = simpleTagParserContainer;
            _markdownWrapper = markdownWrapper;
            _documentTagParser = documentTagParser;
        }


        public virtual ProcessedPayment Build(Payment group)
        {
            var htmlBody = _markdownWrapper.ConvertToHtml(group.Description);
            var processedBody = _parser.ParseAll(htmlBody, group.Name);

            return new ProcessedPayment(group.Name, group.Slug, group.PhoneNumber, group.Email, group.Website, group.Twitter,
                group.Facebook, group.Address, processedBody, group.ImageUrl, group.ThumbnailImageUrl);
        }
    }
}
