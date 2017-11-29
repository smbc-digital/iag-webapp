using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using StockportWebapp.QuestionBuilder.Entities;
using StockportWebapp.QuestionBuilder.Validators;
using Xunit;

namespace StockportWebappTests.Unit.SmartAnswers.Validators
{
    public class CheckboxValidatorTests
    {
        [Theory]
        [InlineData(",test1,test2,test3", "1", false)]
        [InlineData(",test1,test2,test3", "3", true)]
        public void Validate_ShouldPassWhenNumberOfCheckedBoxesIsEqualToValidationValue(string data, string validationValue, bool shouldPass)
        {
            // Arrange
            var question = new Question
            {
                QuestionId = "1",
                QuestionType = "Test",
                Response = data
            };
            var validator = new CheckboxValidator(question, "Check the correct number of checkboxes", validationValue);

            // Act
            var result = validator.Validate(data);

            // Assert
            result.IsValid.Should().Be(shouldPass);
        }

    }
}
