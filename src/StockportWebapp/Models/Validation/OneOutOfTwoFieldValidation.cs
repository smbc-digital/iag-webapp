namespace StockportWebapp.Models.Validation;

[ExcludeFromCodeCoverage]
public class OneOutOfTwoFieldValidation(string propertyName1, string propertyName2) : ValidationAttribute
{
    private readonly string _otherPropertyName1 = propertyName1;
    private readonly string _otherPropertyName2 = propertyName2;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        Type containerType1 = validationContext.ObjectInstance.GetType();
        PropertyInfo field1 = containerType1.GetProperty(_otherPropertyName1, BindingFlags.Public | BindingFlags.Instance);

        object extensionValue1 = field1.GetValue(validationContext.ObjectInstance);
        string firstValue = extensionValue1 as string;

        Type containerType2 = validationContext.ObjectInstance.GetType();
        PropertyInfo field2 = containerType2.GetProperty(_otherPropertyName2, BindingFlags.Public | BindingFlags.Instance);

        object extensionValue2 = field2.GetValue(validationContext.ObjectInstance);
        string secondValue = extensionValue2 as string;

        if (string.IsNullOrWhiteSpace(firstValue) && string.IsNullOrWhiteSpace(secondValue))
            return new ValidationResult("The group email address or group phone number field is required");

        return ValidationResult.Success;
    }
}