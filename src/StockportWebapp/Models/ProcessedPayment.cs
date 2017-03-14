namespace StockportWebapp.Models
{
    public class ProcessedPayment : IProcessedContentType
    {
       
        public readonly string Title;
        public readonly string Slug;
        public readonly string Description;
        public readonly string PaymentDetailsText;
        public readonly string ReferenceLabel;
        public readonly string ParisReference;
        public readonly string Fund;
        public readonly string GlCodeCostCentreNumber;

        public ProcessedPayment()
        { }

        public ProcessedPayment(string title, string slug, string description, string paymentDetailsText,
            string referenceLabel, string parisReference, string fund, string glCodeCostCentreNumber)
        {
            Title = title;
            Slug = slug;
            Description = description;
            PaymentDetailsText = paymentDetailsText;
            ReferenceLabel = referenceLabel;
            ParisReference = parisReference;
            Fund = fund;
            GlCodeCostCentreNumber = glCodeCostCentreNumber;
        }
    }
}
