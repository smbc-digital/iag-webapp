namespace StockportWebapp.QuestionBuilder.Validators
{
    public class ValidationResult
    {
        public ValidationResult(bool isValid, string message, string questionId)
        {
            IsValid = isValid;
            Message = message;
            QuestionId = questionId;
        }

        public readonly bool IsValid;
        public readonly string Message;
        public readonly string QuestionId;

        public static ValidationResult Valid(string questionId)
        {
            return new ValidationResult(true, null, questionId);
        }

        public static ValidationResult Invalid(string message, string questionId)
        {
            return new ValidationResult(false, message, questionId);
        }
    }
}