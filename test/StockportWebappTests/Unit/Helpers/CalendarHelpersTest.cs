using FluentAssertions;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Helpers
{
    public class CalendarHelperTest
    {
        private readonly CalendarHelper _helper;

        public CalendarHelperTest()
        {
            _helper = new CalendarHelper();
        }

        [Fact]
        public void ShouldReturnCorrectCalendarUrlForEventWhenClickOnWindowsOrApple()
        {
            // Arrange
            var eventItem = new Event() { Slug = "test-slug", Description = "Test Description", EventDate = new DateTime(2017, 12, 12), EndTime = "17:00", StartTime = "14:00", Location = "location" };
            var eventUrl = "www.test.com/test-event";
            // Act
            string calenderUrl = _helper.GetIcsText(eventItem, eventUrl);

            // Assert
            calenderUrl.Should().Contain(calenderUrl);
            calenderUrl.Should().Contain("LOCATION:location");
        }

        [Fact]
        public void ShouldReturnCorrectCalendarUrlForEventWhenClickOnGoogle()
        {
            // Arrange
            var eventItem = new Event() { Slug = "test-slug", Description = "Test Description", EventDate = new DateTime(2017, 12, 12), EndTime = "14:00", StartTime = "18:00", Location = "location" };
            var eventUrl = "www.test.com/test-event";
            // Act
            string GoogleCalenderUrl = _helper.GetCalendarUrl(eventItem, eventUrl, "google");

            // Assert
            GoogleCalenderUrl.Should().StartWith("https://www.google.com/calendar/render");
            GoogleCalenderUrl.Should().Be("https://www.google.com/calendar/render?action=TEMPLATE&text=&dates=20171212T180000/20171212T140000&details=For+details,+link+here: www.test.com/test-event &location=location&sf=true&output=xml");
        }

        [Fact]
        public void ShouldReturnCorrectCalendarUrlForEventWhenClickOnYahoo()
        {
            // Arrange
            var eventItem = new Event() { Slug = "test-slug", Description = "Test Description", EventDate = new DateTime(2017, 12, 12), EndTime = "14:00", StartTime = "18:00", Location = "location" };
            var eventUrl = "www.test.com/test-event";
            // Act
            string yahooCalenderUrl = _helper.GetCalendarUrl(eventItem, eventUrl, "yahoo");

            // Assert
            yahooCalenderUrl.Should().StartWith("https://calendar.yahoo.com/");
            yahooCalenderUrl.Should().Be("https://calendar.yahoo.com/?v=60&view=d&type=20&title=&st=20171212T180000&et=20171212T140000&desc=For+details,+link+here: www.test.com/test-event&in_loc=location");
        }
    }
}
