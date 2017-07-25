using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Validators
{
    public abstract class ValidatorBase : IQuestionValidator
    {
        private readonly IQuestion _question;
        private readonly string _validationMessage;
        
        protected ValidatorBase(IQuestion question, string validationMessage)
        {
            _question = question;
            _validationMessage = validationMessage;
        }

        protected ValidatorBase(IQuestion question)
        {
            _question = question;
        }

        public abstract string DefaultValidationMessage { get; }

        public abstract bool IsValid(string input);
        
        public ValidationResult Validate(string input)
        {
            if (input == null)
            {
                return ValidationResult.Invalid(GetValidationMessage(), _question.QuestionId);
            }

            return IsValid(input) ? ValidationResult.Valid(_question.QuestionId) : ValidationResult.Invalid(GetValidationMessage(), _question.QuestionId);
        }

        private string GetValidationMessage()
        {
            return string.IsNullOrEmpty(_validationMessage) ? DefaultValidationMessage : _validationMessage;
        }
        
    }
}