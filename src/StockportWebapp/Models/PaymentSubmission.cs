namespace StockportWebapp.Models;

public class PaymentSubmission
{
    public PaymentSubmission()
    { }

    public PaymentSubmission(ProcessedPayment payment) => Payment = payment;

    public ProcessedPayment Payment { get; set; } = new ProcessedPayment();

    [Required]
    [PaymentReferenceValidation(paymentSubmissionType: EPaymentSubmissionType.Payment)]
    [Display(Name = "reference")]
    public string Reference { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter the amount")]
    [Range(0.01, int.MaxValue, ErrorMessage = "Enter a valid amount (e.g. 25.00)")]
    public decimal? Amount { get; set; }
    
    [RequiredIf("Payment.PaymentType", "Enter a name", "ServicePayPayment")]
    [Display(Name = "name")]
    public string Name { get; set; } = string.Empty;

    [RequiredIfEmailValidation("Payment.PaymentType", "Enter an email address", "ServicePayPayment")]
    [Display(Name = "email address")]
    public string EmailAddress { get; set; } = string.Empty;

    public bool HasServiceSpecifiedPrice =>
        !string.IsNullOrEmpty(Payment.PaymentAmount);
}