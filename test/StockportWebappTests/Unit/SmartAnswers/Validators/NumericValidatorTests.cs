using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using StockportWebapp.QuestionBuilder.Entities;
using StockportWebapp.QuestionBuilder.Validators;
using Xunit;

namespace StockportWebappTests.Unit.SmartAnswers.Validators
{
    public class NumericValidatorTests
    {
        [Theory]
        [InlineData("0123456")]
        [InlineData("0")]
        public void Validate_ShouldPassWhenStringContainsOnlyNumbers(string data)
        {
            // Arrange
            var question = new Mock<IQuestion>();
            var validator = new NumericValidator(question.Object, "This message will not be used");

            // Act
            var actual = validator.Validate(data);

            // Assert
            actual.IsValid.Should().BeTrue();
            actual.Message.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("notanumber")]
        [InlineData(null)]
        public void Validate_ShouldNotPassWhenStringIsNotAnEmail(string data)
        {
            // Arrange
            const string errorMessage = "Must be a number";
            var question = new Mock<IQuestion>();
            var validator = new NumericValidator(question.Object, errorMessage);

            // Act
            var validationResult = validator.Validate(data);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Message.Should().Be(errorMessage);
        }

        [Fact]
        public void Validate_ShouldPassQuestionId_ToValidationResult()
        {
            var questionId = "1";
            var question = new Question
            {
                QuestionId = questionId,
                QuestionType = "Test"
            };
            var validator = new NumericValidator(question, "This message will not be used");

            var actual = validator.Validate("01234567");

            // Assert
            actual.QuestionId.Should().Be(questionId);
        }
    }
}
