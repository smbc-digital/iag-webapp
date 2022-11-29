using FluentAssertions;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Model
{
    public class ProcessedEventsTests
    {
        private readonly ProcessedEvents processedEvent = new ProcessedEvents("title", "slug", "teaser", "image.png", "image.png", "description",
                "fee", "location", "submittedBy", DateTime.Now.AddDays(1), "startTime", "endTime",
                new List<Crumb>(), null, new MapDetails(), "booking information", null, null, null, null);

        [Fact]
        public void ShouldBeTrueIsAlertSunsetDateIsNotPassed()
        {
            var isAlertDisplayed = processedEvent.IsAlertDisplayed(new Alert("title", "subHeading", "body",
                                                                 "severity", DateTime.Now.AddDays(-5), DateTime.Now.AddDays(5), string.Empty, false));
            isAlertDisplayed.Should().Be(true);
        }

        [Fact]
        public void ShouldBeFalseIsAlertSunsetDateIsPassed()
        {
            var isAlertDisplayed = processedEvent.IsAlertDisplayed(new Alert("title", "subHeading", "body",
                                                                 "severity", DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-1), string.Empty, false));
            isAlertDisplayed.Should().Be(false);
        }
    }
}
