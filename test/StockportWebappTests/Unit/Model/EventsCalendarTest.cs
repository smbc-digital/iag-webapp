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
    }
}
