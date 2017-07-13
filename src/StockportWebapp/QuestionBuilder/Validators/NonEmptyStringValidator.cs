using StockportWebapp.Attributes;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Validators
{
    [QuestionValidatorType(QuestionValidatorTypes.NonEmpty)]
    public class NonEmptyStringValidator : ValidatorBase
    {
        public NonEmptyStringValidator(IQuestion question, string validationMessage) : base(question, validationMessage)
        {
        }

        public NonEmptyStringValidator(IQuestion question) : base(question)
        {
        }

        public override bool IsValid(string input)
        {
            var inputWithoutWhitespace = input.Trim();
            return !string.IsNullOrEmpty(inputWithoutWhitespace);
        }

        public override string DefaultValidationMessage => "This is a required answer";
    }
}