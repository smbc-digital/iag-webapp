namespace StockportWebapp.Models.Validation;

[ExcludeFromCodeCoverage]
public class RequiredIfEmailValidation(string otherPropertyName,
                                    string errorMessage,
                                    string expectedValue = "") : ValidationAttribute(errorMessage)
{
    private readonly string _otherPropertyName = otherPropertyName;
    private readonly string _expectedValue = expectedValue;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        Type containerType = validationContext.ObjectInstance.GetType();
        PropertyInfo field = containerType.GetProperty(_otherPropertyName, BindingFlags.Public | BindingFlags.Instance);
        object otherPropertyValue = field?.GetValue(validationContext.ObjectInstance);

        if (string.IsNullOrEmpty(_expectedValue))
        {
            if (otherPropertyValue is bool boolValue && boolValue && string.IsNullOrEmpty(value?.ToString()))
                return new ValidationResult(ErrorMessage);
        }
        else
        {
            if (otherPropertyValue is not null && otherPropertyValue.ToString().Equals(_expectedValue, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(value?.ToString()))
                    return new ValidationResult(ErrorMessage);

                EmailValidation emailValidation = new();
                if (!emailValidation.IsValid(value))
                    return new ValidationResult("Enter a valid email address");
            }
        }

        return ValidationResult.Success;
    }
}