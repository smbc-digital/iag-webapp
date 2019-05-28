using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using StockportWebapp.Enums;
using StockportWebapp.Models;

namespace StockportWebapp.Validation
{
    public class PaymentReferenceValidation : ValidationAttribute
    {
        private static readonly Dictionary<EPaymentReferenceValidation, string> ValidatorsRegex = new Dictionary<EPaymentReferenceValidation, string>
        {
            { EPaymentReferenceValidation.FPN, @"^(\d{5})$" },
            { EPaymentReferenceValidation.ParkingFine, @"^([Ss]{1}[Mm]{1}[34]{1}[0-9]{7})$" },
            { EPaymentReferenceValidation.BusLaneAndCamera, @"^([Ss]{1}[Mm]{1}[8]{1}[0-9]{7})$" },

        };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var paymentSubmission = validationContext.ObjectInstance as PaymentSubmission;

            if (paymentSubmission?.Payment != null)
            {
                if (paymentSubmission.Payment.ReferenceValidation == EPaymentReferenceValidation.None)
                {
                    return ValidationResult.Success;
                }
                var reference = value as string;

                if (reference == null)
                {
                    return new ValidationResult("The reference number is required");
                }

                var isValid = Regex.IsMatch(reference, ValidatorsRegex[paymentSubmission.Payment.ReferenceValidation]);

                if (!isValid)
                {
                    return new ValidationResult("Check the reference number and try again");
                }
            }

            return ValidationResult.Success;
        }
    }
}
