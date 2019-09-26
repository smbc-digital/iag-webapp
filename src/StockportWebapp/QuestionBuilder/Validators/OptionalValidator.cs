using System;
using StockportWebapp.Attributes;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Validators
{
    [QuestionValidatorType(QuestionValidatorTypes.Optional)]
    public class OptionalValidator : ValidatorBase
    {
        public OptionalValidator(IQuestion question, string validationMessage, string validationValue) : base(question, validationMessage, validationValue)
        {
        }

        public OptionalValidator(IQuestion question) : base(question)
        {
        }

        public override bool IsValid(string input, string validationValue)
        {
            validationValue = "true";
            return true;
        }

        public override string DefaultValidationMessage => String.Empty;
    }
}