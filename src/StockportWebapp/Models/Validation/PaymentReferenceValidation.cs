﻿namespace StockportWebapp.Models.Validation;

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
        { EPaymentReferenceValidation.FPN4or5, @"^(\d{4,5})$" },
        { EPaymentReferenceValidation.ParkingFine, @"^([Ss]{1}[Mm]{1}[34]{1}[0-9]{6}[0-9|Aa]{1})$" },
        { EPaymentReferenceValidation.BusLaneAndCamera, @"^([Ss]{1}[Mm]{1}[8]{1}[0-9]{6}[0-9|Aa]{1})$" },
        { EPaymentReferenceValidation.CameraCar, @"^([Ss]{1}[Mm]{1}[6]{1}[0-9]{6}[0-9|Aa]{1})$" },
        { EPaymentReferenceValidation.BusLane, @"^([Ss]{1}[Mm]{1}[8]{1}[0-9]{6}[0-9|Aa]{1})$" },
        { EPaymentReferenceValidation.Applications, @"^([A-Za-z]{2}[A-Za-z0-9\/\\]{6,})$" },
        { EPaymentReferenceValidation.ParkingPermit, @"^([A-Z]{3}\\[0-9]{5})$" },
<<<<<<< sbRegex
        { EPaymentReferenceValidation.BRS, @"^(?i)brs\d{5}$" }
=======
        { EPaymentReferenceValidation.StockportBereavementInvoice, @"^(?i)sb\d{4,5}$" }
>>>>>>> local
    };

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        return _paymentSubmissionType.Equals(EPaymentSubmissionType.Payment)
            ? ProcessPayment(value, validationContext)
            : ProcessServicePayPayment(value, validationContext);
    }

    private ValidationResult ProcessPayment(object value, ValidationContext validationContext)
    {
        var paymentSubmission = validationContext.ObjectInstance as PaymentSubmission; ;

        if (paymentSubmission?.Payment != null)
            return ValidateReference(value, paymentSubmission.Payment.ReferenceValidation);

        return ValidationResult.Success;
    }

    private ValidationResult ProcessServicePayPayment(object value, ValidationContext validationContext)
    {
        var paymentSubmission = validationContext.ObjectInstance as ServicePayPaymentSubmissionViewModel;

        if (paymentSubmission?.Payment != null)
            return ValidateReference(value, paymentSubmission.Payment.ReferenceValidation);

        return ValidationResult.Success;
    }

    private ValidationResult ValidateReference(object value, EPaymentReferenceValidation referenceValidation)
    {
        if (referenceValidation == EPaymentReferenceValidation.None)
            return ValidationResult.Success;

        var reference = value as string;

        if (reference == null)
            return new ValidationResult("The reference number is required");

        var isValid = Regex.IsMatch(reference, ValidatorsRegex[referenceValidation]);

        return !isValid
            ? new ValidationResult("Check the reference number and try again")
            : ValidationResult.Success;
    }
}
