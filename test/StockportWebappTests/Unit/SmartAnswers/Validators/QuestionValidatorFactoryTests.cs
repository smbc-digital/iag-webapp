using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using StockportWebapp.QuestionBuilder.Entities;
using StockportWebapp.QuestionBuilder.Validators;
using StockportWebapp.QuestionBuilder.Validators.Entities;
using Xunit;

namespace StockportWebappTests.Unit.SmartAnswers.Validators
{
    public class QuestionValidatorFactoryTests
    {
        [Fact]
        public void CreateQuestionValidator_ShouldReturnNonEmptyType()
        {
            var validatorData = new ValidatorData
            {
                Type = "non-empty",
                Message = "This is required"
            };
            var question = new Mock<Question>();

            var actual = QuestionValidatorFactory.CreateQuestionValidator(question.Object, validatorData);
            actual.Should().BeOfType<NonEmptyStringValidator>();
        }

        [Fact]
        public void CreateQuestionValidator_ShouldThrowExceptionWithUnknownType()
        {
            var validatorData = new ValidatorData();
            validatorData.Type = "Unknown";
            var question = new Mock<Question>();

            Action exception = () =>
            {
                QuestionValidatorFactory.CreateQuestionValidator(question.Object, validatorData);
            };

            exception.ShouldThrow<ArgumentException>();
        }
    }
}
