using System.Collections.Immutable;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder
{
    public class GenericSmartAnswersQuestions : IGenericSmartAnswersQuestions
    {
        public ImmutableDictionary<int, Page> Structure { get; set; }
    }
}
