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
    public class TelephoneValidatorTests
    {
        [Theory]
        [InlineData("+4416123095742 (330)")]
        [InlineData("01231458735 Ext. 12")]
        [InlineData("0123 1458 735 Ext. 12")]
        [InlineData("01231458735")]
        [InlineData("01231458 735")]
        [InlineData("01231 458 7375")]
        [InlineData("0123 458 7375")]
        [InlineData("012314 58 735")]
        [InlineData("(012314) 58 735")]
        [InlineData("phonenumber 01612")]
        [InlineData("(01) 23 6")]
        [InlineData("01245")]
        public void Validate_ShouldPassTelephoneValidationWhenStringContainsFiveOrMoreDigits(string data)
        {
            var question = new Mock<IQuestion>();
            var validator = new TelephoneValidator(question.Object, "This message will not be used");

            // Act
            var actual = validator.Validate(data);

            // Assert
            actual.IsValid.Should().BeTrue();
            actual.Message.Should().BeNull();
        }

        [Theory]
        [InlineData("0213")]
        [InlineData("werwqer")]
        [InlineData("01 23")]
        [InlineData("(01) 23 Ext")]
        [InlineData("phonenumber 0161")]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_ShouldNotPassTelephoneValidationWhenStringDoesNotContainFiveOrMoreDigits(string data)
        {
            // Arrange
            const string errorMessage = "Must be a telephone number";
            var question = new Mock<IQuestion>();
            var validator = new TelephoneValidator(question.Object, errorMessage);

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
            var validator = new TelephoneValidator(question, "This message will not be used");

            var actual = validator.Validate("0123467567567");

            // Assert
            actual.QuestionId.Should().Be(questionId);
        }
    }
}
