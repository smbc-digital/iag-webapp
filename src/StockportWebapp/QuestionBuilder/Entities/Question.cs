using System.Collections.Generic;
using System.Linq;
using StockportWebapp.QuestionBuilder.Validators;
using StockportWebapp.QuestionBuilder.Validators.Entities;

namespace StockportWebapp.QuestionBuilder.Entities
{
    public class Question : IQuestion
    {
        public string QuestionId { get; set; }
        public string QuestionType { get; set; }
        public string QuestionRenderer { get; set; }
        public string Label { get; set; }
        public string SecondaryInfo { get; set; }
        public IList<QuestionOption> Options { get; set; }
        public string Response { get; set; }
        public IList<ValidatorData> ValidatorData { get; set; } = new List<ValidatorData>();
        public ValidationResult ValidationResult { get; set; }
        public IList<IQuestionValidator> Validators => ValidatorData.Select(validatorData => QuestionValidatorFactory.CreateQuestionValidator(this, validatorData)).ToList();

        public void Validate()
        {
            ValidationResult = (Validators == null) ? ValidationResult.Valid(QuestionId) : Validators.Aggregate(ValidationResult.Valid(QuestionId), (outResult, validator) => !outResult.IsValid ? outResult : validator.Validate(Response));
        }
    }
}