using System;
using FluentAssertions;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using System.Collections.Generic;
using Xunit;

namespace StockportWebappTests.Unit.Helpers
{
    public class StatusColourHelperTest
    {

        public StatusColourHelperTest()
        {
        }

        [Theory]
        [InlineData("Published", "green")]
        [InlineData("Archived", "red")]
        [InlineData("", "green")]
        public void ShouldReturnCorrectColour(string status, string colour)
        {
            // Arrange
            string returnColour;

            // Act
            returnColour = StatusColourHelper.GetStatusColour(status);

            // Assert
            returnColour.Should().Be(colour);
        }
    }
}
