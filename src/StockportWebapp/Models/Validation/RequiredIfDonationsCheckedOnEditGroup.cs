namespace StockportWebapp.Models.Validation;

[ExcludeFromCodeCoverage]
public class RequiredIfDonationsCheckedOnEditGroup : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        GroupSubmission model = (GroupSubmission)validationContext.ObjectInstance;
        if (!model.Donations)
            return ValidationResult.Success;

        string stringValue = value as string;
        string displayName = validationContext.DisplayName;

        return string.IsNullOrEmpty(stringValue)
            ? new ValidationResult($"Enter a {displayName}.")
            : ValidationResult.Success;
    }
}