using System.Collections.Generic;
using StockportWebapp.Enums;

namespace StockportWebapp.QuestionBuilder.Entities
{
    public interface IBehaviour
    {
        EQuestionType BehaviourType { get; set; }
        IList<Condition> Conditions { get; set; }
        string Value { get; set; }
        string RedirectValue { get; set; }
        bool ShouldTrigger(IList<Answer> responses);
    }
}