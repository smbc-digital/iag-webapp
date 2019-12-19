using System.Collections.Generic;
using StockportWebapp.Enums;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedPayment : IProcessedContentType
    {
       
        public readonly string Title;
        public string Slug;
        public readonly string Teaser;
        public readonly string Description;
        public readonly string PaymentDetailsText;
        public readonly string ReferenceLabel;
        public readonly string ParisReference;
        public readonly string Fund;
        public readonly string GlCodeCostCentreNumber;
        public readonly List<Crumb> Breadcrumbs;
        public readonly EPaymentReferenceValidation ReferenceValidation;
        public readonly string MetaDescription;
        public readonly string ReturnUrl;
        public readonly string CatalogueId;
        public readonly string PaymentDescription;
        public ProcessedPayment()
        { }

        public ProcessedPayment(string title, string slug, string teaser, string description, string paymentDetailsText,
            string referenceLabel, string parisReference, string fund, string glCodeCostCentreNumber, List<Crumb> breadcrumbs,
            EPaymentReferenceValidation referenceValidation, string metaDescription, string returnUrl, string catalogueId, string paymentDescription)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            Description = description;
            PaymentDetailsText = paymentDetailsText;
            ReferenceLabel = referenceLabel;
            ParisReference = parisReference;
            Fund = fund;
            GlCodeCostCentreNumber = glCodeCostCentreNumber;
            Breadcrumbs = breadcrumbs;
            ReferenceValidation = referenceValidation;
            MetaDescription = metaDescription;
            ReturnUrl = returnUrl;
            CatalogueId = catalogueId;
            PaymentDescription = paymentDescription;
        }
    }
}
