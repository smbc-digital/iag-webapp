namespace StockportWebapp.ContentFactory;

public class ServicePayPaymentFactory
{
    private readonly ISimpleTagParserContainer _simpleTagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper;

    public ServicePayPaymentFactory(ISimpleTagParserContainer simpleTagParserContainer, MarkdownWrapper markdownWrapper)
    {
        _simpleTagParserContainer = simpleTagParserContainer;
        _markdownWrapper = markdownWrapper;
    }

    public virtual ProcessedServicePayPayment Build(ServicePayPayment payment)
    {
        var description = _simpleTagParserContainer.ParseAll(payment.Description, payment.Title);
        description = _markdownWrapper.ConvertToHtml(description ?? "");

        return new ProcessedServicePayPayment(
            payment.Title,
            payment.Slug,
            payment.Teaser,
            description,
            payment.PaymentDetailsText,
            payment.ReferenceLabel,
            payment.BreadCrumbs,
            payment.ReferenceValidation,
            payment.MetaDescription,
            payment.ReturnUrl,
            payment.CatalogueId,
            payment.AccountReference,
            payment.PaymentDescription,
            payment.Alerts,
            payment.PaymentAmount);
    }
}
