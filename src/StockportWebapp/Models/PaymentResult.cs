namespace StockportWebapp.Models
{
    public class PaymentResult
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public List<Crumb> Breadcrumbs { get; set; } = new List<Crumb>();

        public PaymentResult() { }
    }
}
