using System.ComponentModel.DataAnnotations;
using StockportWebapp.Enums;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Validation;

namespace StockportWebapp.ViewModels
{
    public class ServicePayPaymentSubmissionViewModel
    {
        public ProcessedServicePayPayment Payment { get; set; } = new ProcessedServicePayPayment();

        [Required]
        [PaymentReferenceValidation(paymentSubmissionType: EPaymentSubmissionType.ServicePayPayment)]
        [Display(Name = "reference")]
        public string Reference { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter the payment amount in numbers")]
        [Range(0.01, int.MaxValue, ErrorMessage = "Enter the payment amount in numbers")]
        public decimal? Amount { get; set; } = 0;

        [Required]
        [Display(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailValidation]
        [Display(Name = "email address")]
        public string EmailAddress { get; set; } = string.Empty;

        public bool HasServiceSpecifiedPrice => !string.IsNullOrEmpty(Payment.PaymentAmount);
    }
}
