namespace StockportWebapp.Models
{
    public class PaymentResult
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string ReceiptNumber { get; set; }
        public List<Crumb> Breadcrumbs { get; set; } = new List<Crumb>();
        public PaymentResultType PaymentResultType { get; set; }

        public PaymentResult() { }
        public PaymentResult(string slug, string title, List<Crumb> breadcrumbs, string receiptNumber) 
        { 
            Slug = slug;
            Title = title;
            Breadcrumbs = breadcrumbs;
            ReceiptNumber = receiptNumber;
        }
    }

    public enum PaymentResultType
    {
        Success,
        Failure,
        Declined
    }
}
