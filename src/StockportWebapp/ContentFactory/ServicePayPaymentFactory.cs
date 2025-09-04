namespace StockportWebapp.ContentFactory;

public class ServicePayPaymentFactory(ITagParserContainer simpleTagParserContainer, MarkdownWrapper markdownWrapper)
{
    private readonly ITagParserContainer _tagParserContainer = simpleTagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

    public virtual ProcessedServicePayPayment Build(ServicePayPayment payment)
    {
        string description = _tagParserContainer.ParseAll(payment.Description, payment.Title);
        description = _markdownWrapper.ConvertToHtml(description ?? string.Empty);

        return new(payment.Title,
                payment.Slug,
                payment.Teaser,
                description,
                _markdownWrapper.ConvertToHtml(payment.PaymentDetailsText ?? string.Empty),
                payment.ReferenceLabel,
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