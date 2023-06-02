namespace StockportWebapp.Models.Validation;

public class EndDateLaterThanStartDateValidation : ValidationAttribute
{
    private readonly string _otherPropertyName;
    private readonly string _errorMessage;

    public EndDateLaterThanStartDateValidation(string otherPropertyName, string errorMessage) : base(errorMessage)
    {
        _otherPropertyName = otherPropertyName;
        _errorMessage = errorMessage;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var containerType = validationContext.ObjectInstance.GetType();
        var field = containerType.GetProperty(_otherPropertyName, BindingFlags.Public | BindingFlags.Instance);
        var extensionValue = field?.GetValue(validationContext.ObjectInstance);
        var startDate = extensionValue as DateTime?;

        var date = value as DateTime?;
        if (!date.HasValue || !startDate.HasValue) return ValidationResult.Success;
        if (date.Value.Date >= startDate.Value.Date) return ValidationResult.Success;
        return new ValidationResult(_errorMessage);
    }
}
