namespace StockportWebapp.Models.Validation;

public class PastDateValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        DateTime.TryParse(value.ToString(), out DateTime date);

        return date > DateTime.Now
            ? new ValidationResult("Dates must be in the past")
            : ValidationResult.Success;
    }
}