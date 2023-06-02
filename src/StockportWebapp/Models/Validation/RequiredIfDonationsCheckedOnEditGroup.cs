namespace StockportWebapp.Models.Validation;

public class RequiredIfDonationsCheckedOnEditGroup : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var model = (GroupSubmission)validationContext.ObjectInstance;
        if (!model.Donations)
        {
            return ValidationResult.Success;
        }

        var stringValue = value as string;
        var displayName = validationContext.DisplayName;

        return string.IsNullOrEmpty(stringValue)
            ? new ValidationResult($"Enter a {displayName}.")
            : ValidationResult.Success;
    }
}
