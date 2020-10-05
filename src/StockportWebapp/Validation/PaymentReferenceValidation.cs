using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using StockportWebapp.Enums;
using StockportWebapp.Models;

namespace StockportWebapp.Validation
{
    public class PaymentReferenceValidation : ValidationAttribute
    {
        private readonly EPaymentSubmissionType _paymentSubmissionType;
        public PaymentReferenceValidation(EPaymentSubmissionType paymentSubmissionType)
        {
            _paymentSubmissionType = paymentSubmissionType;
        }
        
        private static readonly Dictionary<EPaymentReferenceValidation, string> ValidatorsRegex = new Dictionary<EPaymentReferenceValidation, string>
        {
            { EPaymentReferenceValidation.FPN, @"^(\d{5})$" },
            { EPaymentReferenceValidation.ParkingFine, @"^([Ss]{1}[Mm]{1}[34]{1}[0-9]{6}[0-9|Aa]{1})$" },
            { EPaymentReferenceValidation.BusLaneAndCamera, @"^([Ss]{1}[Mm]{1}[8]{1}[0-9]{6}[0-9|Aa]{1})$" },
            { EPaymentReferenceValidation.CameraCar, @"^([Ss]{1}[Mm]{1}[6]{1}[0-9]{6}[0-9|Aa]{1})$" },
            { EPaymentReferenceValidation.BusLane, @"^([Ss]{1}[Mm]{1}[8]{1}[0-9]{6}[0-9|Aa]{1})$" },
            { EPaymentReferenceValidation.Applications, @"^([A-Za-z]{2}[A-Za-z0-9\/\\]{6,})$" },
        };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return _paymentSubmissionType.Equals(EPaymentSubmissionType.Payment)
                ? ProcessPayment(value, validationContext)
                : ProcessServicePayPayment(value, validationContext);
        }

        private ValidationResult ProcessPayment(object value, ValidationContext validationContext)
        {
            var paymentSubmission = validationContext.ObjectInstance as PaymentSubmission;;

            if (paymentSubmission?.Payment != null)
               return ValidateReference(value, paymentSubmission.Payment.ReferenceValidation);

            return ValidationResult.Success;
        }

        private ValidationResult ProcessServicePayPayment(object value, ValidationContext validationContext)
        {
            var paymentSubmission = validationContext.ObjectInstance as ServicePayPaymentSubmission;

            if (paymentSubmission?.Payment != null)
                return ValidateReference(value, paymentSubmission.Payment.ReferenceValidation);

            return ValidationResult.Success;
        }

        private ValidationResult ValidateReference(object value, EPaymentReferenceValidation referenceValidation)
        {
            if (referenceValidation == EPaymentReferenceValidation.None)
            {
                return ValidationResult.Success;
            }
            var reference = value as string;

            var isValid = Regex.IsMatch(reference, ValidatorsRegex[referenceValidation]);

            return !isValid 
                ? new ValidationResult("Check the reference number and try again") 
                : ValidationResult.Success;
        }
    }
}
