namespace StockportWebapp.QuestionBuilder.Validators
{
    public interface IQuestionValidator
    {
        ValidationResult Validate(string input);
    }
}