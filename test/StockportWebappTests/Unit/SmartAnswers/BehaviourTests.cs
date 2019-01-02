using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using StockportWebapp.Enums;
using StockportWebapp.QuestionBuilder.Entities;
using Xunit;

namespace StockportWebappTests_Unit.Unit.SmartAnswers
{
    public class BehaviourTests
    {
        [Fact]
        public void ShouldTrigger_ShouldReturnTrueWhenAnswerMatches()
        {
            var behaviour = new Behaviour
            {
                BehaviourType = EQuestionType.GoToPage,
                Conditions = new List<Condition>
                {
                    new Condition
                    {
                        QuestionId = "1",
                        EqualTo = "Answer"
                    }
                },
                Value = "0"
            };

            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = "1",
                    Response = "Answer"
                }
            };

            var actual = behaviour.ShouldTrigger(answers);
            actual.Should().BeTrue();
        }

        [Fact]
        public void ShouldTrigger_ShouldReturnFalseWhenAnswerDoesNotMatch()
        {
            var behaviour = new Behaviour
            {
                BehaviourType = EQuestionType.GoToPage,
                Conditions = new List<Condition>
                {
                    new Condition
                    {
                        QuestionId = "2",
                        EqualTo = "Answer"
                    }
                },
                Value = "0"
            };

            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = "2",
                    Response = "Answer1"
                }
            };

            var actual = behaviour.ShouldTrigger(answers);
            actual.Should().BeFalse();
        }

        [Fact]
        public void ShouldTrigger_ShouldReturnFalseWhenQuestionIdDoesNotMatch()
        {
            var behaviour = new Behaviour
            {
                BehaviourType = EQuestionType.GoToPage,
                Conditions = new List<Condition>
                {
                    new Condition
                    {
                        QuestionId = "3",
                        EqualTo = "Answer"
                    }
                },
                Value = "0"
            };

            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = "4",
                    Response = "Answer"
                }
            };

            var actual = behaviour.ShouldTrigger(answers);
            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData("10","0,20",true)]
        [InlineData("20","0,20",true)]
        [InlineData("30","0,20",false)]
        [InlineData("30","0,max",true)]
        [InlineData("0","0,max",true)]
        [InlineData("200","max,123",false)]
        public void ShouldTrigger_ShouldTriggerBetweenCondition_AndReturnCorrectBoolValue(string value, string condition, bool expected)
        {
            // Arrange
            var behaviour = new Behaviour
            {
                BehaviourType = EQuestionType.GoToPage,
                Conditions = new List<Condition>
                {
                    new Condition
                    {
                        QuestionId = "2",
                        Between = condition
                    }
                },
                Value = "0"
            };

            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = "2",
                    Response = value
                }
            };

            // Act
            var actual = behaviour.ShouldTrigger(answers);

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("10", "20", false)]
        [InlineData("20", "20", true)]
        public void ShouldTrigger_ShouldTriggerEqualTo_AndReturnCorrectBoolValue(string value, string condition, bool expected)
        {
            // Arrange
            var behaviour = new Behaviour
            {
                BehaviourType = EQuestionType.GoToPage,
                Conditions = new List<Condition>
                {
                    new Condition
                    {
                        QuestionId = "2",
                        EqualTo = condition
                    }
                },
                Value = "0"
            };

            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = "2",
                    Response = value
                }
            };

            // Act
            var actual = behaviour.ShouldTrigger(answers);

            // Assert
            actual.Should().Be(expected);
        }

    }
}
