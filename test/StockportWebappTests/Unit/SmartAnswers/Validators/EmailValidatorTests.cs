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
    public class EmailValidatorTests
    {
        [Theory]
        [InlineData("test@test.com")]
        [InlineData("joe.bloggs@stockport.gov.uk")]
        [InlineData("joe.bloggs3@companywithnumber2.museum")]
        public void Validate_ShouldPassWhenStringIsAnEmail(string data)
        {
            // Arrange
            var question = new Mock<IQuestion>();
            var validator = new EmailValidator(question.Object, "This message will not be used");

            // Act
            var actual = validator.Validate(data);

            // Assert
            actual.IsValid.Should().BeTrue();
            actual.Message.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("notanemail")]
        [InlineData("notanemail.com")]
        [InlineData(null)]
        public void Validate_ShouldNotPassWhenStringIsNotAnEmail(string data)
        {
            // Arrange
            const string errorMessage = "First Name cannot be empty";
            var question = new Mock<IQuestion>();
            var validator = new EmailValidator(question.Object, errorMessage);

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
            var validator = new EmailValidator(question, "This message will not be used");

            var actual = validator.Validate("simon@test.com");

            // Assert
            actual.QuestionId.Should().Be(questionId);
        }
    }
}
