using System.Collections.Generic;
using StockportWebapp.Enums;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedServicePayPayment : IProcessedContentType
    {
        public readonly string Title;
        public string Slug;
        public readonly string Teaser;
        public readonly string Description;
        public readonly string PaymentDetailsText;
        public readonly string ReferenceLabel;
        public readonly List<Crumb> Breadcrumbs;
        public readonly EPaymentReferenceValidation ReferenceValidation;
        public readonly string MetaDescription;
        public readonly string ReturnUrl;
        public readonly string CatalogueId;
        public readonly string AccountReference;
        public readonly string PaymentDescription;
        public readonly IEnumerable<Alert> Alerts;
        public readonly string PaymentAmount;

        public ProcessedServicePayPayment()
        { }

        public ProcessedServicePayPayment(string title, string slug, string teaser, string description, string paymentDetailsText,
            string referenceLabel, List<Crumb> breadcrumbs, EPaymentReferenceValidation referenceValidation,
            string metaDescription, string returnUrl, string catalogueId, string accountReference,
            string paymentDescription, IEnumerable<Alert> alerts, string paymentAmount)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            Description = description;
            PaymentDetailsText = paymentDetailsText;
            ReferenceLabel = referenceLabel;
            Breadcrumbs = breadcrumbs;
            ReferenceValidation = referenceValidation;
            MetaDescription = metaDescription;
            ReturnUrl = returnUrl;
            CatalogueId = catalogueId;
            AccountReference = accountReference;
            PaymentDescription = paymentDescription;
            Alerts = alerts;
            PaymentAmount = paymentAmount;
        }
    }
}