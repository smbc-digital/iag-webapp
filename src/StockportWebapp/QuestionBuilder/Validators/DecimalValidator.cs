using System.Text.RegularExpressions;
using StockportWebapp.Attributes;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Validators
{
    [QuestionValidatorType(QuestionValidatorTypes.Decimal)]
    public class DecimalValidator : ValidatorBase
    {
        public DecimalValidator(IQuestion question, string validationMessage) : base(question, validationMessage)
        {
        }

        public DecimalValidator(IQuestion question) : base(question)
        {
        }

        public override bool IsValid(string input)
        {
            Regex numericRegex = new Regex(@"^\d+(\.\d{1,2})?$");
            return numericRegex.IsMatch(input);
        }

        public override string DefaultValidationMessage => "Enter a valid number to 2 decimal places";
    }
}