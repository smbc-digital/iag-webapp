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
            dateCalculator.Today().Should().Be("2016-08-02");
        }

        [Fact]
        public void ShouldGetTomorrowDate()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.Tomorrow().Should().Be("2016-08-03");
        }

        [Fact]
        public void ShouldGetNearestFriday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestFriday().Should().Be("2016-08-05");
        }

        [Fact]
        public void ShouldGetTodayAsNearestFridayIfTodayIsFriday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 05));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestFriday().Should().Be("2016-08-05");
        }

        [Fact]
        public void ShouldGetTodayAsNearestFridayIfTodayIsSaturday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 06));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestFriday().Should().Be("2016-08-06");
        }

        [Fact]
        public void ShouldGetTodayAsNearestFridayIfTodayIsSunday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 07));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestFriday().Should().Be("2016-08-07");
        }

        [Fact]
        public void ShouldGetNearestSunday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestSunday().Should().Be("2016-08-07");
        }

        [Fact]
        public void ShouldGetTodayAsNearestSundayIfTodayIsSunday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 07));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestSunday().Should().Be("2016-08-07");
        }

        [Fact]
        public void ShouldGetNearestMonday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestMonday().Should().Be("2016-08-08");
        }

        [Fact]
        public void ShouldGetNextMondayIfTodayIsMonday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 01));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NearestMonday().Should().Be("2016-08-08");
        }

        [Fact]
        public void ShouldGetNextSunday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NextSunday().Should().Be("2016-08-14");
        }

        [Fact]
        public void ShouldGetNextSundayIfTodayIsSunday()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 07));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.NextSunday().Should().Be("2016-08-14");
        }

        [Fact]
        public void ShouldGetLastDayOfMonth()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.LastDayOfMonth().Should().Be("2016-08-31");
        }

        [Fact]
        public void ShouldGetTodayAsLastDayOfMonth()
        {
            _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 31));
            DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
            dateCalculator.LastDayOfMonth().Should().Be("2016-08-31");
        }
    }
}
