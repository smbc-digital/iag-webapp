using System.Text.RegularExpressions;
using StockportWebapp.Attributes;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Validators
{
    [QuestionValidatorType(QuestionValidatorTypes.Telephone)]
    public class TelephoneValidator : ValidatorBase
    {
        public TelephoneValidator(IQuestion question, string validationMessage) : base(question, validationMessage)
        {
        }

        public TelephoneValidator(IQuestion question) : base(question)
        {
        }

        public override bool IsValid(string input)
        {
            Regex numericRegex = new Regex(@"\D*(?:\d\D*){5}$");
            return numericRegex.IsMatch(input);
        }

        public override string DefaultValidationMessage => "Enter a valid telephone number";
    }
}