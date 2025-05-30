﻿namespace StockportWebapp.Models.Validation;

[ExcludeFromCodeCoverage]
public class RequiredIfPaymentSubmission(EPaymentSubmissionType paymentSubmissionType,
                        string errorMessage,
                        string expectedValue = "") : ValidationAttribute(errorMessage)
{
    private readonly EPaymentSubmissionType _paymentSubmissionType = paymentSubmissionType;
    private readonly string _expectedValue = expectedValue;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is not PaymentSubmission paymentSubmission)
            return ValidationResult.Success;

        bool shouldValidate = ShouldValidate(paymentSubmission) && string.IsNullOrEmpty(value?.ToString());
        return shouldValidate
            ? new ValidationResult(ErrorMessage)
            : ValidationResult.Success;
    }

    private bool ShouldValidate(PaymentSubmission paymentSubmission)
    {
        if (_paymentSubmissionType != EPaymentSubmissionType.ServicePayPayment)
            return false;

        PropertyInfo paymentProperty = paymentSubmission.GetType().GetProperty(nameof(paymentSubmission.Payment), BindingFlags.Public | BindingFlags.Instance);
        object paymentInstance = paymentProperty?.GetValue(paymentSubmission);

        FieldInfo field = paymentInstance?.GetType().GetField(nameof(Payment.PaymentType), BindingFlags.Public | BindingFlags.Instance);
        string propertyValue = field?.GetValue(paymentInstance)?.ToString();

        return !string.IsNullOrEmpty(_expectedValue)
            ? propertyValue?.Equals(_expectedValue, StringComparison.OrdinalIgnoreCase) == true
            : bool.TryParse(propertyValue, out var boolValue) && boolValue;
    }
}