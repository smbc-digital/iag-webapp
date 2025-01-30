namespace StockportWebapp.Models.Validation;

[ExcludeFromCodeCoverage]
public class EndTimeLaterThanStartTimeValidation(string otherPropertyName,
                                                string errorMessage) : ValidationAttribute(errorMessage)
{
    private readonly string _otherPropertyName = otherPropertyName;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        Type containerType = validationContext.ObjectInstance.GetType();
        PropertyInfo field = containerType.GetProperty(_otherPropertyName, BindingFlags.Public | BindingFlags.Instance);

        object extensionValue = field.GetValue(validationContext.ObjectInstance);
        DateTime? startTime = extensionValue as DateTime?;

        if (!startTime.HasValue)
            return new ValidationResult("Should enter valid Start Time");

        if (value is null)
            return ValidationResult.Success;

        DateTime? endTime = value as DateTime?;
        if (!endTime.HasValue)
            return new ValidationResult("Should enter valid End Time");

        if (endTime.Value > startTime.Value)
            return ValidationResult.Success;

        return new ValidationResult("End Time should be after Start Time");
    }
}