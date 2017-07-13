using System.Text.RegularExpressions;
using StockportWebapp.Attributes;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Validators
{
    [QuestionValidatorType(QuestionValidatorTypes.Numeric)]
    public class NumericValidator : ValidatorBase
    {
        public NumericValidator(IQuestion question, string validationMessage) : base(question, validationMessage)
        {
        }

        public NumericValidator(IQuestion question) : base(question)
        {
        }

        public override bool IsValid(string input)
        {
            Regex numericRegex = new Regex(@"^([0-9])+$");
            return numericRegex.IsMatch(input);
        }

        public override string DefaultValidationMessage => "Enter a valid number";
    }
}