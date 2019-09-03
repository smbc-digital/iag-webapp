using System.Collections.Generic;
using System.Linq;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.Utils
{
    public interface ISmartAnswerStringHelper
    {
        string DescriptionTextParser(string description, IList<Answer> prevAnswers);
    }

    public class SmartAnswerStringHelper : ISmartAnswerStringHelper
    {
        public string DescriptionTextParser(string description, IList<Answer> prevAnswers)
        {
            var openBoi = description.Count(x => x == '{');
            var closedBoi = description.Count(x => x == '}');
            if (openBoi != closedBoi)
            {
                return description;
            }

            if (openBoi > 1)
            {
                var descSpilt = description.Split('}');
                descSpilt = descSpilt.Take(descSpilt.Count() - 1).ToArray();

                foreach (var decSplitRow in descSpilt)
                {
                    var indDescSplit = decSplitRow.Replace("{", "").Replace("}", "").Split(':');

                    foreach (Answer answer in prevAnswers)
                    {
                        if (answer.QuestionId == indDescSplit[0] && answer.Response == indDescSplit[1])
                        {
                            description = indDescSplit[2];
                            return description;
                        }
                    }

                }
                return description;
            }
            else
            {
                var indDescSplit = description.Replace("{", "").Replace("}", "").Split(':');
                if (indDescSplit.Length == 2)
                {
                    foreach (Answer answer in prevAnswers)
                    {
                        if (answer.QuestionId == indDescSplit[1].Trim())
                        {
                            description = indDescSplit[0] + ": " + answer.Response;
                            return description;
                        }
                    }
                    return description;
                }
                else
                {
                    foreach (Answer answer in prevAnswers)
                    {
                        if (answer.QuestionId == indDescSplit[0] && answer.Response == indDescSplit[1])
                        {
                            description = indDescSplit[2];
                            return description;
                        }
                    }
                    return description;

                }

            }
        }
    }
}
