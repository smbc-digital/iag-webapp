using StockportWebapp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Moq;

namespace StockportWebappTests.Unit.Utils
{
    public class ViewHelpersTest
    {
        private readonly Mock<ITimeProvider> _timeProvider;

        public ViewHelpersTest()
        {
            _timeProvider = new Mock<ITimeProvider>();
            _timeProvider.Setup(o => o.Now()).Returns(new DateTime(2017, 02, 1));
        }

        [Theory]
        [InlineData(10, "09:30", "Saturday 11 February at 9:30am")]
        [InlineData(0, "09:30", "Today at 9:30am")]
        [InlineData(1, "09:30", "Tomorrow at 9:30am")]
        [InlineData(10, "19:30", "Saturday 11 February at 7:30pm")]
        public void FormatEventDateShouldReturnCorrectDateString(int daysOffset, string time, string expected)
        {
            // Arrange
            var date = _timeProvider.Object.Now().AddDays(daysOffset);
            var viewHelper = new ViewHelpers(_timeProvider.Object);

            // Act
            var result = viewHelper.FormatEventDate(date, time);

            // Assert
            result.Should().Be(expected);
        }
    }
}
