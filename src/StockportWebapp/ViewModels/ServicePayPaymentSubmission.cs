namespace StockportWebapp.ViewModels;

public class ServicePayPaymentSubmissionViewModel
{
    public ProcessedServicePayPayment Payment { get; set; } = new();

    [PaymentReferenceValidation(paymentSubmissionType: EPaymentSubmissionType.ServicePayPayment, ErrorMessage = "Check the reference and try again")]
    [Display(Name = "reference")]
    public string Reference { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter a payment amount")]
    [Range(0.01, int.MaxValue, ErrorMessage = "Enter a valid amount (e.g. 25.00)")]
    public string Amount { get; set; }

    [Required(ErrorMessage = "Enter your name")]
    [Display(Name = "name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter your email address")]
    [EmailValidation]
    [Display(Name = "email address")]
    public string EmailAddress { get; set; } = string.Empty;
}