namespace StockportWebapp.Models.Validation
{
    public class PaymentReferenceValidation : ValidationAttribute
    {
        private readonly EPaymentSubmissionType _paymentSubmissionType;
        private static readonly Dictionary<EPaymentReferenceValidation, string> ValidatorsRegex = new()
        {
            { EPaymentReferenceValidation.FPN, @"^(\d{5})$" },
            { EPaymentReferenceValidation.FPN4or5, @"^(\d{4,5})$" },
            { EPaymentReferenceValidation.ParkingFine, @"^([Ss]{1}[Mm]{1}[34]{1}[0-9]{6}[0-9|Aa]{1})$" },
            { EPaymentReferenceValidation.BusLaneAndCamera, @"^([Ss]{1}[Mm]{1}[8]{1}[0-9]{6}[0-9|Aa]{1})$" },
            { EPaymentReferenceValidation.CameraCar, @"^([Ss]{1}[Mm]{1}[6]{1}[0-9]{6}[0-9|Aa]{1})$" },
            { EPaymentReferenceValidation.BusLane, @"^([Ss]{1}[Mm]{1}[8]{1}[0-9]{6}[0-9|Aa]{1})$" },
            { EPaymentReferenceValidation.Applications, @"^([A-Za-z]{2}[A-Za-z0-9\/\\]{6,})$" },
            { EPaymentReferenceValidation.ParkingPermit, @"^([A-Z]{2}[A-Z0-9]{1}\\[0-9]{5})$" },
            { EPaymentReferenceValidation.StockportBereavementInvoice, @"^(?i)sb\d{4,5}$" },
            { EPaymentReferenceValidation.PlanningApplication, @"^(?i)(dc\/\d{6}|enq\/\d{6}|ps\/[\w\s]{1,10})$" }
        };

        public PaymentReferenceValidation(EPaymentSubmissionType paymentSubmissionType)
        {
            _paymentSubmissionType = paymentSubmissionType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (_paymentSubmissionType.Equals(EPaymentSubmissionType.Payment))
                return ProcessPayment(value, validationContext);
            return ProcessServicePayPayment(value, validationContext);
        }

        private ValidationResult ProcessPayment(object value, ValidationContext validationContext)
        {
            var paymentSubmission = validationContext.ObjectInstance as PaymentSubmission;
            if (paymentSubmission?.Payment != null)
                return ValidateReference(value, paymentSubmission.Payment.ReferenceValidation, paymentSubmission.Payment.ReferenceLabel);
            return ValidationResult.Success;
        }

        private ValidationResult ProcessServicePayPayment(object value, ValidationContext validationContext)
        {
            var paymentSubmission = validationContext.ObjectInstance as ServicePayPaymentSubmissionViewModel;
            if (paymentSubmission?.Payment != null)
                return ValidateReference(value, paymentSubmission.Payment.ReferenceValidation, paymentSubmission.Payment.ReferenceLabel);
            
            return ValidationResult.Success;
        }

        private ValidationResult ValidateReference(object value, EPaymentReferenceValidation referenceValidation, string referenceLabel)
        {
            if (referenceValidation.Equals(EPaymentReferenceValidation.None))
                return ValidationResult.Success;

            string reference = value as string;

            if (string.IsNullOrEmpty(reference))
                return new ValidationResult($"Enter the {referenceLabel.ToLower()}");

            bool isValid = Regex.IsMatch(reference, ValidatorsRegex[referenceValidation]);

            return !isValid
                ? new ValidationResult($"Check the {referenceLabel.ToLower()} and try again")
                : ValidationResult.Success;
        }
    }
}