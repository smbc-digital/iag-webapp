using System;
using System.ComponentModel.DataAnnotations;


namespace StockportWebapp.Validation
{
    public class FutureDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           
            if (value == null) return ValidationResult.Success;
            var date = DateTime.Parse(value.ToString());
            var today = DateTime.Today;
            if (date.Date > today)
                return ValidationResult.Success;
            return new  ValidationResult("Should be a future date");
        }
    }
}
