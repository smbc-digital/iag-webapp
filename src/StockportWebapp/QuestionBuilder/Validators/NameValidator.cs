using System.Text.RegularExpressions;
using StockportWebapp.Attributes;
using StockportWebapp.Constants;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Validators
{
    [QuestionValidatorType(QuestionValidatorTypes.Name)]
    public class NameValidator : ValidatorBase
    {
        public NameValidator(IQuestion question, string validationMessage, string validationValue) : base(question, validationMessage, validationValue)
        {
        }

        public NameValidator(IQuestion question) : base(question)
        {
        }

        public override bool IsValid(string input, string validationValue)
        {
            Regex regex = new Regex(ValidationConstants.ValidNameCharacters);
            return regex.IsMatch(input);
        }

        public override string DefaultValidationMessage => "Name contains invalid characters";
    }
}