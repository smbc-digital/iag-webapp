namespace StockportWebapp.Models.Validation;

[ExcludeFromCodeCoverage]
public class RequiredIfEmailValidation(EPaymentSubmissionType paymentSubmissionType, string errorMessage, string expectedValue = "") : ValidationAttribute(errorMessage)
{
    private readonly EPaymentSubmissionType _paymentSubmissionType = paymentSubmissionType;
    private readonly string _expectedValue = expectedValue;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is not PaymentSubmission paymentSubmission)
            return ValidationResult.Success;

        return ShouldValidate(paymentSubmission, value)
            ? new ValidationResult(ErrorMessage)
            : ValidationResult.Success;
    }

    private bool ShouldValidate(PaymentSubmission paymentSubmission, object value)
    {
        if (_paymentSubmissionType != EPaymentSubmissionType.ServicePayPayment)
            return false;

        PropertyInfo paymentProperty = paymentSubmission.GetType().GetProperty(nameof(paymentSubmission.Payment), BindingFlags.Public | BindingFlags.Instance);
        object paymentInstance = paymentProperty?.GetValue(paymentSubmission);

        FieldInfo field = paymentInstance?.GetType().GetField(nameof(Payment.PaymentType), BindingFlags.Public | BindingFlags.Instance);
        string propertyValue = field?.GetValue(paymentInstance)?.ToString();

        if (!string.IsNullOrEmpty(_expectedValue) && propertyValue?.Equals(_expectedValue, StringComparison.OrdinalIgnoreCase) == true)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
                return true;

            Regex emailRegex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$", RegexOptions.Compiled);
            return !emailRegex.IsMatch(value.ToString());
        }

        return false;
    }
}