using System.Collections.Generic;
using System.Linq;
using StockportWebapp.QuestionBuilder.Entities;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Utils
{
    public class SmartAnswerStringHelperTest
    {
        //This is to test the description field in smart answers in contentful json.
        [Theory]
        [InlineData("{complainingAbout:bins:You're complaining about bins}", "You're complaining about bins")]
        [InlineData("{complainingAbout:cows:You're complaining about cows}{complainingAbout:biyns:You're complaining about bins}{complainingAbout:ducks:You're complaining about ducks}", "{complainingAbout:cows:You're complaining about cows}{complainingAbout:biyns:You're complaining about bins}{complainingAbout:ducks:You're complaining about ducks}")]
        [InlineData("{complainingAbout:cows:You're complaining about cows}{complainingAbout:bins:You're complaining about bins}{complainingAbout:ducks:You're complaining about ducks}", "You're complaining about bins")]
        [InlineData("You're complaining about: {complainingAbout}", "You're complaining about: bins")]
        [InlineData("You're on about: {complainingAbout}", "You're on about: bins")]
        [InlineData("Hello this is the description. The answer previously was: {complainingAbout}", "Hello this is the description. The answer previously was: bins")]
        public void replaceSingleSpecialTextWithJustTheRightWordsNoQuestionID(string description, string expected)
        {
            //Arrange
            IList<Answer> PrevAnswers = new List<Answer>();
            PrevAnswers.Add(new Answer() { QuestionId = "complainingAbout", QuestionText = "What you got an issue with", Response = "bins", ResponseValue = "ducks" });
            PrevAnswers.Add(new Answer() { QuestionId = "forename", QuestionText = "What forname?", Response = "barry", ResponseValue = "barry" });

            //Act
            description = checkSpecialText(description, PrevAnswers);
            //Assert
            Assert.Equal(expected, description);
        }

        private string checkSpecialText(string description, IList<Answer> prevAnswers)
        {
            var openBoi = description.Count(x => x == '{');
            var closedBoi = description.Count(x => x == '}');
            if (openBoi != closedBoi)
            {
                description = "curlyBoi's not right";
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
