using StockportWebapp.Attributes;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Validators
{
    [QuestionValidatorType(QuestionValidatorTypes.MaxLength)]
    public class MaxLengthValidator : ValidatorBase
    {
        public MaxLengthValidator(IQuestion question, string validationMessage, string validationValue) : base(question, validationMessage, validationValue)
        {
        }

        public MaxLengthValidator(IQuestion question) : base(question)
        {
        }

        public override bool IsValid(string input, string validationValue)
        {
            var maxLength = int.Parse(validationValue);

            return input.Length <= maxLength;
        }

        public override string DefaultValidationMessage => "This is a required answer";
    }
}
