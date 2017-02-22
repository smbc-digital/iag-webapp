using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using StockportWebapp.Models;

namespace StockportWebappTests.Unit.Model
{
    public class EventsCalendarTest
    {
        [Fact]
        public void ShouldReturnTrueForExistingCategory()
        {
            var eventCalendar = new EventCalendar(new List<Event>(), new List<string> {"test", "test2"});

            eventCalendar.DoesCategoryExist("test").Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnFalseForNonExistingCategory()
        {
            var eventCalendar = new EventCalendar(new List<Event>(), new List<string> { "test1", "test2" });

            eventCalendar.DoesCategoryExist("test").Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnDateRangeForCustomDate()
        {
            var eventCalendar = new EventCalendar
            {
                DateRange = "customdate",
                DateFrom = new DateTime(2016, 01, 01),
                DateTo = new DateTime(2016, 01, 10)
            };

            var result = eventCalendar.GetCustomEventFilterName();

            result.Should().Be("01/01/2016 to 10/01/2016");
        }

        [Fact]
        public void ShouldReturnEmptyStringForBlankDates()
        {
            var eventCalendar = new EventCalendar();

            var result = eventCalendar.GetCustomEventFilterName();

            result.Should().Be(string.Empty);
        }
    }
}
