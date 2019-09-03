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
        public string RedirectValue { get; set; }
        
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
            double enteredValue;
            if (!double.TryParse(value, out enteredValue)) return false;

            var splitValues = condition.Split(',');

            if (splitValues.Length != 2) return false;

            splitValues = HandleTextInBetweenValues(splitValues);

            var betweenValues = splitValues.Select(double.Parse).ToList();

            return enteredValue >= betweenValues[0] && enteredValue <= betweenValues[1];
        }

        private static string[] HandleTextInBetweenValues(IEnumerable<string> splitValues)
        {
            return splitValues.Select(_ =>
            {
                double valueAsNumber;
                if (!double.TryParse(_, out valueAsNumber))
                {
                    return int.MaxValue.ToString();
                }

                return _;

            }).ToArray();
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