using System.Text.RegularExpressions;
using StockportWebapp.Attributes;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Validators
{
    [QuestionValidatorType(QuestionValidatorTypes.Email)]
    public class EmailValidator : ValidatorBase
    {
  
        public EmailValidator(IQuestion question, string validationMessage) : base(question, validationMessage)
        {
        }

        public EmailValidator(IQuestion question) : base(question)
        {
        }

        public override bool IsValid(string input)
        {
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
            return emailRegex.IsMatch(input);
        }

        public override string DefaultValidationMessage => "Enter a valid email address";
    }
}