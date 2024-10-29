﻿namespace StockportWebapp.Models.Validation;

[ExcludeFromCodeCoverage]
public class RequiredIf : ValidationAttribute
{
    private readonly string _otherPropertyName;
    private readonly string _errorMessage;

    public RequiredIf(string otherPropertyName, string errorMessage) : base(errorMessage)
    {
        _otherPropertyName = otherPropertyName;
        _errorMessage = errorMessage;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var containerType = validationContext.ObjectInstance.GetType();
        var field = containerType.GetProperty(_otherPropertyName, BindingFlags.Public | BindingFlags.Instance);
        var extensionValue = field?.GetValue(validationContext.ObjectInstance);

        if ((bool)extensionValue)
        {
            if (value is null)
                return new ValidationResult(_errorMessage);
        }

        return ValidationResult.Success;
    }
}
