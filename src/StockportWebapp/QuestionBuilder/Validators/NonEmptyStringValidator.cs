using StockportWebapp.Attributes;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Validators
{
    [QuestionValidatorType(QuestionValidatorTypes.NonEmpty)]
    public class NonEmptyStringValidator : ValidatorBase
    {
        public NonEmptyStringValidator(IQuestion question, string validationMessage, string validationValue) : base(question, validationMessage, validationValue)
        {
        }

        public NonEmptyStringValidator(IQuestion question) : base(question)
        {
        }

        public override bool IsValid(string input, string validationValue)
        {
            var inputWithoutWhitespace = input.Trim();
            return !string.IsNullOrEmpty(inputWithoutWhitespace);
        }

        public override string DefaultValidationMessage => "This is a required answer";
    }
}