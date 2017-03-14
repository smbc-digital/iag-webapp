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
        }


        public virtual ProcessedPayment Build(Payment payment)
        {
            var description = _simpleTagParserContainer.ParseAll(payment.Description, payment.Title);
            description = _markdownWrapper.ConvertToHtml(description ?? "");
            

            return new ProcessedPayment(
                payment.Title,
                payment.Slug,
                description,
                payment.PaymentDetailsText,
                payment.ReferenceLabel,
                payment.ParisReference,
                payment.Fund,
                payment.GlCodeCostCentreNumber,
                payment.BreadCrumbs);
        }
    }
}
