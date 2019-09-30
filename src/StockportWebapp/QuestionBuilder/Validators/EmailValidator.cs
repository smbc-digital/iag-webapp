using System.Text.RegularExpressions;
using Quartz.Util;
using StockportWebapp.Attributes;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Validators
{
    [QuestionValidatorType(QuestionValidatorTypes.Email)]
    public class EmailValidator : ValidatorBase
    {
  
        public EmailValidator(IQuestion question, string validationMessage, string validationValue) : base(question, validationMessage, validationValue)
        {
        }

        public EmailValidator(IQuestion question) : base(question)
        {
        }

        public override bool IsValid(string input, string validationValue)
        {
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
            return input != null && emailRegex.IsMatch(input);
        }

        public override string DefaultValidationMessage => "Enter a valid email address";
    }
}