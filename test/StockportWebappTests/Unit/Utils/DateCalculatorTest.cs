using AngleSharp.Parser.Html;
using Xunit;
using FluentAssertions;
using StockportWebapp.Utils;
using Moq;
using System;

namespace StockportWebappTests.Unit.Utils
{
    public class DateCalculatorTest
    {
        private readonly Mock<ITimeProvider> _mockTimeProvider;

        public DateCalculatorTest()
        {
            _mockTimeProvider = new Mock<ITimeProvider>();
        }

        [Fact]
        public void ShouldGetTodayDate()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.Today().Should().Be("02/08/2016");
        }

        [Fact]
        public void ShouldGetTomorrowDate()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.Tomorrow().Should().Be("03/08/2016");
        }

        [Fact]
        public void ShouldGetNearestFriday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestFriday().Should().Be("05/08/2016");
        }

        [Fact]
        public void ShouldGetTodayAsNearestFridayIfTodayIsFriday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 05));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestFriday().Should().Be("05/08/2016");
        }

        [Fact]
        public void ShouldGetTodayAsNearestFridayIfTodayIsSaturday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 06));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestFriday().Should().Be("06/08/2016");
        }

        [Fact]
        public void ShouldGetTodayAsNearestFridayIfTodayIsSunday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 07));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestFriday().Should().Be("07/08/2016");
        }

        [Fact]
        public void ShouldGetNearestSunday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestSunday().Should().Be("07/08/2016");
        }

        [Fact]
        public void ShouldGetTodayAsNearestSundayIfTodayIsSunday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 07));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestSunday().Should().Be("07/08/2016");
        }

        [Fact]
        public void ShouldGetNearestMonday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestMonday().Should().Be("08/08/2016");
        }

        [Fact]
        public void ShouldGetNextMondayIfTodayIsMonday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 01));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestMonday().Should().Be("08/08/2016");
        }

        [Fact]
        public void ShouldGetNextSunday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NextSunday().Should().Be("14/08/2016");
        }

        [Fact]
        public void ShouldGetNextSundayIfTodayIsSunday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 07));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NextSunday().Should().Be("14/08/2016");
        }

        [Fact]
        public void ShouldGetLastDayOfMonth()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.LastDayOfMonth().Should().Be("31/08/2016");
        }

        [Fact]
        public void ShouldGetTodayAsLastDayOfMonth()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 31));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.LastDayOfMonth().Should().Be("31/08/2016");
        }

        [Fact]
        public void ShouldGetFistDayOfTheNextMonth()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2017, 01, 25));
            var dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.FirstDayOfNextMonth().Should().Be("01/02/2017");
        }

        [Fact]
        public void ShouldGetLastDayOfTheNextMonth()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2017, 01, 25));
            var dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.LastDayOfNextMonth().Should().Be("28/02/2017");
        }

        [Fact]
        public void ShouldGetFirstDayOfTheNextMonthInNewYear()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2017, 12, 25));
            var dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.FirstDayOfNextMonth().Should().Be("01/01/2018");
        }

        [Fact]
        public void ShouldGetLastDayOfTheNextMonthInNewYear()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2017, 12, 25));
            var dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.LastDayOfNextMonth().Should().Be("31/01/2018");
        }

        [Fact]
        public void ShouldGetValueForKeyForFilterIfKeyExists()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2017, 12, 25));
            var dateCalculator = new DateCalculator(_mockTimeProvider.Object);

            var value = dateCalculator.ReturnDisplayNameForFilter("today");

            value.Should().Be("Today");
        }

        [Fact]
        public void ShouldGetValueForKeyForFilterIfKeyExistsForAnotherKey()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2017, 12, 25));
            var dateCalculator = new DateCalculator(_mockTimeProvider.Object);

            var value = dateCalculator.ReturnDisplayNameForFilter("thisweekend");

            value.Should().Be("This weekend");
        }

        [Fact]
        public void ShoulNotGiveAValueForNoExistantKey()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2017, 12, 25));
            var dateCalculator = new DateCalculator(_mockTimeProvider.Object);

            var value = dateCalculator.ReturnDisplayNameForFilter("none");

            value.Should().Be(string.Empty);
        }
    }
}
