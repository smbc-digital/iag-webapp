namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class PaymentResult(string slug, string title, List<Crumb> breadcrumbs, string receiptNumber, bool isServicePay)
{
    public string Slug { get; set; } = slug;
    public string Title { get; set; } = title;
    public string ReceiptNumber { get; set; } = receiptNumber;
    public bool IsServicePay { get; set; } = isServicePay;
    public List<Crumb> Breadcrumbs { get; set; } = breadcrumbs;
    public PaymentResultType PaymentResultType { get; set; }
}

public enum PaymentResultType
{
    Success,
    Failure,
    Declined
}