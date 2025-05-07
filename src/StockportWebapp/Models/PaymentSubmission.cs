namespace StockportWebapp.Models;

public class PaymentSubmission
{
    public PaymentSubmission()
    { }

    public PaymentSubmission(ProcessedPayment payment) => Payment = payment;

    public ProcessedPayment Payment { get; set; } = new ProcessedPayment();

    [PaymentReferenceValidation(paymentSubmissionType: EPaymentSubmissionType.Payment, ErrorMessage = "Check the reference and try again")]
    [Display(Name = "reference")]
    public string Reference { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter a payment amount")]
    [Range(0.01, int.MaxValue, ErrorMessage = "Enter a valid amount (e.g. 25.00)")]
    public string? Amount { get; set; }
    
    [RequiredIfPaymentSubmission(EPaymentSubmissionType.ServicePayPayment, "Enter your name", "ServicePayPayment")]
    [Display(Name = "name")]
    public string Name { get; set; } = string.Empty;

    [RequiredIfEmailValidation(EPaymentSubmissionType.ServicePayPayment, "Enter your email address", "ServicePayPayment")]
    [Display(Name = "email address")]
    public string EmailAddress { get; set; } = string.Empty;
}