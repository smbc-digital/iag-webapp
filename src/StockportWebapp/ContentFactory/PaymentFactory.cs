﻿namespace StockportWebapp.ContentFactory;

public class PaymentFactory
{
    private readonly ITagParserContainer _tagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper;

    public PaymentFactory(ITagParserContainer simpleTagParserContainer, MarkdownWrapper markdownWrapper)
    {
        _tagParserContainer = simpleTagParserContainer;
        _markdownWrapper = markdownWrapper;
    }

    public virtual ProcessedPayment Build(Payment payment)
    {
        var description = _tagParserContainer.ParseAll(payment.Description, payment.Title);
        description = _markdownWrapper.ConvertToHtml(description ?? "");

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
