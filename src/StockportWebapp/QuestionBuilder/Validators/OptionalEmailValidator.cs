using System.Text.RegularExpressions;
using StockportWebapp.Attributes;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Validators
{
    [QuestionValidatorType(QuestionValidatorTypes.OptionalEmail)]
    public class OptionalEmailValidator : ValidatorBase
    {
  
        public OptionalEmailValidator(IQuestion question, string validationMessage, string validationValue) : base(question, validationMessage, validationValue)
        {
        }

        public OptionalEmailValidator(IQuestion question) : base(question)
        {
        }

        public override bool IsValid(string input, string validationValue)
        {
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
            var isValid = string.IsNullOrEmpty(input) || emailRegex.IsMatch(input);
            return isValid;
        }

        public override string DefaultValidationMessage => "Enter a valid email address";
    }
}