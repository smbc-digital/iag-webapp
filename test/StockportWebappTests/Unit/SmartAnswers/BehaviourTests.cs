using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using StockportWebapp.Enums;
using StockportWebapp.QuestionBuilder.Entities;
using Xunit;

namespace StockportWebappTests.Unit.SmartAnswers
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
    }
}
