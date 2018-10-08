using System.ComponentModel.DataAnnotations;
using StockportWebapp.Models;

namespace StockportWebapp.Validation
{
    public class RequiredIfVolunteeringCheckedOnEditGroup : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (GroupSubmission)validationContext.ObjectInstance;
            if (!model.Volunteering)
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
}
