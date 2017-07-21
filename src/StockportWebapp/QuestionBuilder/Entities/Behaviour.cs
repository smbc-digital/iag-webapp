using System;
using System.Collections.Generic;
using System.Linq;
using StockportWebapp.Enums;

namespace StockportWebapp.QuestionBuilder.Entities
{
    public class Behaviour : IBehaviour
    {
        public EQuestionType BehaviourType { get; set; }
        public IList<Condition> Conditions { get; set; }
        public string Value { get; set; }
        
        public bool ShouldTrigger(IList<Answer> responses)
        {
            // Conditions must all match for Behaviour to trigger
            if (Conditions != null && Conditions.Count > 0)
            {
                var didConditionMatch = true;
                foreach (var condition in Conditions)
                {
                    var matchedQuestion = responses.FirstOrDefault(r => r.QuestionId == condition.QuestionId);
                    if (matchedQuestion != null &&
                        String.Equals(matchedQuestion.Response, condition.EqualTo,
                            StringComparison.CurrentCultureIgnoreCase) && didConditionMatch)
                    {
                        didConditionMatch = true;
                    }
                    else
                    {
                        didConditionMatch = false;
                    }
                }
                return didConditionMatch;
            }

            // If we don't have any conditions on the behaviour, it should match
            return true;
        }
    }
}