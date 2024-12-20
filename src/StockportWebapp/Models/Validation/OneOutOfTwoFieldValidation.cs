﻿namespace StockportWebapp.Models.Validation;

[ExcludeFromCodeCoverage]
public class OneOutOfTwoFieldValidation : ValidationAttribute
{
    private readonly string _otherPropertyName1;
    private readonly string _otherPropertyName2;

    public OneOutOfTwoFieldValidation(string propertyName1, string propertyName2)
    {
        _otherPropertyName1 = propertyName1;
        _otherPropertyName2 = propertyName2;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var containerType1 = validationContext.ObjectInstance.GetType();
        var field1 = containerType1.GetProperty(_otherPropertyName1, BindingFlags.Public | BindingFlags.Instance);

        var extensionValue1 = field1.GetValue(validationContext.ObjectInstance);
        var firstValue = extensionValue1 as string;

        var containerType2 = validationContext.ObjectInstance.GetType();
        var field2 = containerType2.GetProperty(_otherPropertyName2, BindingFlags.Public | BindingFlags.Instance);

        var extensionValue2 = field2.GetValue(validationContext.ObjectInstance);
        var secondValue = extensionValue2 as string;

        if (string.IsNullOrWhiteSpace(firstValue) && string.IsNullOrWhiteSpace(secondValue))
            return new ValidationResult("The group email address or group phone number field is required");

        return ValidationResult.Success;
    }
}
