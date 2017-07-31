using System.Collections.Generic;
using System.Linq;

namespace StockportWebapp.QuestionBuilder.Entities
{
    public class Answer
    {
        public string QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string Response { get; set; }
        public string ResponseValue { get; set; }
    }

    public static class ExtensionMethods
    {
        public static Answer GetAnswerToQuestion(this IList<Answer> answers, string questionId)
        {
            return answers.FirstOrDefault(a => a.QuestionId == questionId);
        }
    }
}