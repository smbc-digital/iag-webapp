using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockportWebapp.Models
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

        public ProcessedPayment()
        { }

        public ProcessedPayment(string title, string slug, string teaser, string description, string paymentDetailsText,
            string referenceLabel, string parisReference, string fund, string glCodeCostCentreNumber, List<Crumb> breadcrumbs)
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
        }
    }
}
