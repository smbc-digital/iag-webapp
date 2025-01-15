namespace StockportWebapp.Models.Validation;

[ExcludeFromCodeCoverage]
public class EndDateLaterThanStartDateValidation(string otherPropertyName,
                                                string errorMessage) : ValidationAttribute(errorMessage)
{
    private readonly string _otherPropertyName = otherPropertyName;
    private readonly string _errorMessage = errorMessage;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        Type containerType = validationContext.ObjectInstance.GetType();
        PropertyInfo field = containerType.GetProperty(_otherPropertyName, BindingFlags.Public | BindingFlags.Instance);
        object extensionValue = field?.GetValue(validationContext.ObjectInstance);
        DateTime? startDate = extensionValue as DateTime?;
        DateTime? date = value as DateTime?;

        if (!date.HasValue || !startDate.HasValue)
            return ValidationResult.Success;
        
        if (date.Value.Date >= startDate.Value.Date)
            return ValidationResult.Success;

        return new ValidationResult(_errorMessage);
    }
}