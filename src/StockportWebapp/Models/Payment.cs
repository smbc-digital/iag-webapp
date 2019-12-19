using System.Collections.Generic;
using StockportWebapp.Enums;

namespace StockportWebapp.Models
{
    public class Payment
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Teaser { get; set; }
        public string Description { get; set; }
        public string PaymentDetailsText { get; set; }
        public string ReferenceLabel { get; set; }
        public string ParisReference { get; set; }
        public string Fund { get; set; }
        public string GlCodeCostCentreNumber { get; set; }
        public string Icon { get; set; }
        public List<Crumb> BreadCrumbs { get; set; }
        public EPaymentReferenceValidation ReferenceValidation { get; set; }
        public string MetaDescription { get; set; }
        public string ReturnUrl { get; set; }
        public string CatalogueId { get; set; }
        public string PaymentDescription { get; set; }
    }
}
