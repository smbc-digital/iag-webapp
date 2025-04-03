namespace StockportWebapp.Models.Validation;

[ExcludeFromCodeCoverage]
public class RequiredIf(string otherPropertyName, string errorMessage) : ValidationAttribute(errorMessage)
{
    private readonly string _otherPropertyName = otherPropertyName;
    private readonly string _errorMessage = errorMessage;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        Type containerType = validationContext.ObjectInstance.GetType();
        PropertyInfo field = containerType.GetProperty(_otherPropertyName, BindingFlags.Public | BindingFlags.Instance);
        object extensionValue = field?.GetValue(validationContext.ObjectInstance);

        if ((bool)extensionValue && value is null)
            return new ValidationResult(_errorMessage);

        return ValidationResult.Success;
    }
}