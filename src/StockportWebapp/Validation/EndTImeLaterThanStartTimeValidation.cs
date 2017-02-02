using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace StockportWebapp.Validation
{
    public class EndTimeLaterThanStartTimeValidation : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public EndTimeLaterThanStartTimeValidation(string otherPropertyName, string erroMessgae) : base(erroMessgae)
        {
            _otherPropertyName = otherPropertyName;

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(_otherPropertyName, BindingFlags.Public | BindingFlags.Instance) ;
            
            var extensionValue = field.GetValue(validationContext.ObjectInstance);
            var startTime = DateTime.Parse(extensionValue.ToString());  

            if(startTime == DateTime.MinValue)
                return new ValidationResult("Should enter valid Time");

            if (value == null) return ValidationResult.Success;
            var endTime = DateTime.Parse(value.ToString());
           
            if (endTime > startTime)
                return ValidationResult.Success;
            return new ValidationResult("End Time should be after Start Time");
        }
    }
}
