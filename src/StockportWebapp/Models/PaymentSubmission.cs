﻿namespace StockportWebapp.Models;

public class PaymentSubmission
{
    public PaymentSubmission()
    { }

    public PaymentSubmission(ProcessedPayment payment) => Payment = payment;

    public ProcessedPayment Payment { get; set; } = new ProcessedPayment();

    [PaymentReferenceValidation(paymentSubmissionType: EPaymentSubmissionType.Payment)]
    public string Reference { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter a payment amount")]
    [Range(0.01, int.MaxValue, ErrorMessage = "Enter a valid amount (e.g. 25.00)")]
    public string Amount { get; set; }
}