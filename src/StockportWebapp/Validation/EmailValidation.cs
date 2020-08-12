using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using StockportWebapp.Models;

namespace StockportWebapp.Validation
{
    public class EmailValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
            var paymentSubmission = validationContext.ObjectInstance as ServicePayPaymentSubmission;

            if (string.IsNullOrEmpty(paymentSubmission?.EmailAddress))
                return new ValidationResult("The email address is required");

            return Regex.IsMatch(paymentSubmission.EmailAddress, emailRegex.ToString())
                ? ValidationResult.Success
                : new ValidationResult("Check the email address and try again");
        }
    }
}