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
                foreach (var condition in Conditions)
                {
                    var matchedQuestion = responses.FirstOrDefault(r => r.QuestionId == condition.QuestionId);
                    if (matchedQuestion != null)
                    {
                        if (!string.IsNullOrEmpty(condition.EqualTo))
                        {
                            return IsEqualTo(matchedQuestion.Response, condition.EqualTo);
                        }

                        if (!string.IsNullOrEmpty(condition.Between))
                        {
                            return IsBetween(matchedQuestion.Response, condition.Between);
                        }
                    }
                    return false;
                }
            }

            // If we don't have any conditions on the behaviour, it should match
            return true;
        }

        public bool IsBetween(string value, string condition)
        {
            int enteredValue;
            if (int.TryParse(value, out enteredValue))
            {
                string[] splitValues = condition.Split(',');

                if (splitValues[1] == "max")
                    splitValues[1] = int.MaxValue.ToString();

                List<int> betweenValues = splitValues.Select(int.Parse).ToList();

                if (enteredValue >= betweenValues[0] && enteredValue <= betweenValues[1])
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsEqualTo(string value, string condition)
        {
            if (String.Equals(value, condition,
                StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}