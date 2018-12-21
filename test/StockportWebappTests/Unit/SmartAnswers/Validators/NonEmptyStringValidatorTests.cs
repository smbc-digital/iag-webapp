using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using StockportWebapp.QuestionBuilder.Entities;
using StockportWebapp.QuestionBuilder.Validators;
using Xunit;

namespace StockportWebappTests_Unit.Unit.SmartAnswers.Validators
{
    public class NonEmptyStringValidatorTests
    {
        [Theory]
        [InlineData("Something")]
        [InlineData("another thing")]
        public void Validate_ShouldPassWhenStringIsNotEmpty(string data)
        {
            // Arrange
            var question = new Mock<IQuestion>();
            var validator = new NonEmptyStringValidator(question.Object, "This message will not be used", null);

            // Act
            var actual = validator.Validate(data);

            // Assert
            actual.IsValid.Should().BeTrue();
            actual.Message.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData("\t\t\t")]
        [InlineData("\n")]
        [InlineData(null)]
        public void Validate_ShouldNotPassWhenStringIsEmptyOrNullOrWhitespace(string data)
        {
            // Arrange
            const string errorMessage = "First Name cannot be empty";
            var question = new Mock<IQuestion>();
            var validator = new NonEmptyStringValidator(question.Object, errorMessage, null);

            // Act
            var validationResult = validator.Validate(data);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Message.Should().Be(errorMessage);
        }
    }
}
