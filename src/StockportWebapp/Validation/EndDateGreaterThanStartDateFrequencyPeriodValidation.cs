using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace StockportWebapp.Validation
{
    public class EndDateGreaterThanStartDateFrequencyPeriodValidation : ValidationAttribute
    {
        private readonly string _otherPropertyName;
        private readonly string _frequencyPropertyName;

        public EndDateGreaterThanStartDateFrequencyPeriodValidation(string otherPropertyName, string frequencyPropertyName, string errorMessage) : base(errorMessage)
        {
            _otherPropertyName = otherPropertyName;
            _frequencyPropertyName = frequencyPropertyName;

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(_otherPropertyName, BindingFlags.Public | BindingFlags.Instance) ;
            var extensionValue = field.GetValue(validationContext.ObjectInstance);
            var startDate = extensionValue as DateTime?;

           
            var frequencyField = containerType.GetProperty(_frequencyPropertyName, BindingFlags.Public | BindingFlags.Instance);
            var frequencyValue = frequencyField.GetValue(validationContext.ObjectInstance);
            var frequency = frequencyValue as string;

            if (!startDate.HasValue)
                return new ValidationResult("Should enter valid Start Date");
            //"Daily", "Weekly", "Fortnightly", "Monthly Date","Monthly Day","Yearly"
           
            var endDate = value as DateTime?;
            if (!endDate.HasValue)
                  return ValidationResult.Success;

            var validationDate = startDate.Value;
            string validationMessage = "End date should be after Start Date";
            switch (frequency)
            {
                case "Daily":
                    validationDate = validationDate.Date.AddDays(1);
                    validationMessage = "End Date should be at least one day after Start Date";
                    break;
                case "Weekly":
                    validationDate = validationDate.Date.AddDays(7);
                    validationMessage = "End Date should be at least one weak after Start Date";
                    break;
                case "Fortnightly":
                    validationDate = validationDate.Date.AddDays(14);
                    validationMessage = "End Date should be at least one fortnight after Start Date";
                    break;
                case "Monthly Date":
                    validationDate = validationDate.Date.AddMonths(1);
                    validationMessage = "End Date should be at least one month after Start Date";
                    break;
                case "Monthly Day":
                    validationDate = validationDate.Date.AddMonths(1);
                    validationMessage = "End Date should be at least one month after Start Date";
                    break;
                case "Yearly":
                    validationDate = validationDate.Date.AddYears(1);
                    validationMessage = "End Date should be at least one year after Start Date";
                    break;
                default:
                    return ValidationResult.Success;
            }
            if (endDate.Value.Date >= validationDate)
                return ValidationResult.Success;
            return new ValidationResult(validationMessage);
        }
    }
}
