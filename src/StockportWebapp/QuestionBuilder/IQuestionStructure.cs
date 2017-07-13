using System.Collections.Immutable;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder
{
    public interface IQuestionStructure
    {
        ImmutableDictionary<int, Page> Structure { get; set; }
    }
}
