﻿namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class PaymentSuccess
{
    public string Title { get; set; }
    public string ReceiptNumber { get; set; }
    public string MetaDescription { get; set; }
    public List<Crumb> Breadcrumbs { get; set; } = new List<Crumb>();
}