using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Validation;

namespace StockportWebapp.Models
{
    public class ServicePayPaymentSubmission
    {
        public ProcessedServicePayPayment Payment { get; set; } = new ProcessedServicePayPayment();

        [Required]
        [PaymentReferenceValidation]
        public string Reference { get; set; } = string.Empty;

        [Required]
        [Range(0.01, int.MaxValue, ErrorMessage = "Please enter a valid amount (e.g. 25.00)")]
        public decimal Amount { get; set; } = 0;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailValidation]
        [DisplayName("Email address")]
        public string EmailAddress { get; set; } = string.Empty;
    }
}
