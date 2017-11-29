using System.Text.RegularExpressions;
using StockportWebapp.Attributes;
using StockportWebapp.QuestionBuilder.Entities;


namespace StockportWebapp.QuestionBuilder.Validators
{
    [QuestionValidatorType(QuestionValidatorTypes.Checkbox)]
    public class CheckboxValidator : ValidatorBase
    {
        public CheckboxValidator(IQuestion question, string validationMessage, string validationValue) : base(question, validationMessage, validationValue)
        {
        }

        public CheckboxValidator(IQuestion question) : base(question)
        {
        }

        public override bool IsValid(string input, string value)
        {
            int.TryParse(value, out var checkboxValidationCount);
            var trimStartingComma = input.TrimStart(',');
            var checkedBoxes = trimStartingComma.Split(',').Length;

            return checkboxValidationCount == checkedBoxes;
        }

        public override string DefaultValidationMessage => "Enter a valid number to 2 decimal places";
    }
}