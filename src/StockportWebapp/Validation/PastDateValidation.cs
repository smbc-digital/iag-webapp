using System;
using System.ComponentModel.DataAnnotations;

namespace StockportWebapp.Validation
{
    public class PastDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) { return ValidationResult.Success;}
            
            DateTime date;
            DateTime.TryParse(value.ToString(), out date);

            return date > DateTime.Now ? new ValidationResult("Dates must be in the past") : ValidationResult.Success;
        }
    }
}
