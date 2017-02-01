using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace StockportWebapp.Validation
{
    public class EndDateLaterThanStartDateValidation : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public EndDateLaterThanStartDateValidation(string otherPropertyName, string erroMessgae) : base(erroMessgae)
        {
            _otherPropertyName = otherPropertyName;

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(_otherPropertyName, BindingFlags.Public | BindingFlags.Instance) ;
            var extensionValue = field.GetValue(validationContext.ObjectInstance);
            var startDate = DateTime.Parse(extensionValue.ToString());  

            if(startDate == DateTime.MinValue)
                return new ValidationResult("Should enter valid Start Date");

            if (value == null) return ValidationResult.Success;
            var date = DateTime.Parse(value.ToString());
           
            if (date.Date >= startDate.Date)
                return ValidationResult.Success;
            return new ValidationResult("End date should be after Start Date");
        }
    }
}
