namespace StockportWebapp.Models
{
    public class PaymentResponse
    {
        public string Title { get; set; }
        public string TransactionType { get; set; }
        public string Amount { get; set; }
        public string AdministrationCharge { get; set; }
        public string Data { get; set; }
        public string ServiceProcessed { get; set; }
        public string MerchantNumber { get; set; }
        public string AuthorisationCode { get; set; }
        public string Date { get; set; }
        public string MerchantTid { get; set; }
        public string ReceiptNumber { get; set; }
        public string Hash { get; set; }
        public string Slug { get; set; }
        public string MetaDescription { get; set; }
    }
}
