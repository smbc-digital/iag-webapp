namespace StockportWebapp.ContentFactory;

public class PaymentFactory(ITagParserContainer simpleTagParserContainer,
                            MarkdownWrapper markdownWrapper)
{
    private readonly ITagParserContainer _tagParserContainer = simpleTagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

    public virtual ProcessedPayment Build(Payment payment)
    {
        string description = _tagParserContainer.ParseAll(payment.Description, payment.Title);
        description = _markdownWrapper.ConvertToHtml(description ?? string.Empty);

        return new ProcessedPayment(
            payment.Title,
            payment.Slug,
            payment.Teaser,
            description,
            payment.PaymentDetailsText,
            payment.ReferenceLabel,
            payment.Fund,
            payment.GlCodeCostCentreNumber,
            payment.BreadCrumbs,
            payment.ReferenceValidation,
            payment.MetaDescription,
            payment.ReturnUrl,
            payment.CatalogueId,
            payment.AccountReference,
            payment.PaymentDescription,
            payment.Alerts);
    }
}