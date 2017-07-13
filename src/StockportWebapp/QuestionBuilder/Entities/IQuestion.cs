using System.Collections.Generic;
using StockportWebapp.QuestionBuilder.Validators;
using StockportWebapp.QuestionBuilder.Validators.Entities;

namespace StockportWebapp.QuestionBuilder.Entities
{
    public interface IQuestion
    {
        string QuestionId { get; set; }
        string QuestionType { get; set; }
        string QuestionRenderer { get; set; }
        string Label { get; set; }
        string SecondaryInfo { get; set; }
        IList<QuestionOption> Options { get; set; }
        string Response { get; set; }
        IList<ValidatorData> ValidatorData { get; set; }
        IList<IQuestionValidator> Validators { get; }
        ValidationResult ValidationResult { get; set; }
        void Validate();
    }
}