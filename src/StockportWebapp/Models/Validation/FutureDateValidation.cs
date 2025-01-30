namespace StockportWebapp.Models.Validation;

[ExcludeFromCodeCoverage]
public class FutureDateValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        DateTime? date = value as DateTime?;
        if (!date.HasValue)
            return ValidationResult.Success;

        DateTime today = DateTime.Today;
        if (date.Value.Date > today)
            return ValidationResult.Success;

        return new ValidationResult("Should be a future date");
    }
}