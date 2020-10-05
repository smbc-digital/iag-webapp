using System.ComponentModel.DataAnnotations;
using StockportWebapp.Enums;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Validation;

namespace StockportWebapp.Models
{
    public class ServicePayPaymentSubmission
    {
        public ProcessedServicePayPayment Payment { get; set; } = new ProcessedServicePayPayment();

        [Required]
        [PaymentReferenceValidation(paymentSubmissionType: EPaymentSubmissionType.ServicePayPayment)]
        [Display(Name = "reference")]
        public string Reference { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a valid amount (e.g. 25.00)")]
        [Range(0.01, int.MaxValue, ErrorMessage = "Please enter a valid amount (e.g. 25.00)")]
        public decimal? Amount { get; set; } = 0;

        [Required]
        [Display(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailValidation]
        [Display(Name = "email address")]
        public string EmailAddress { get; set; } = string.Empty;
    }
}
