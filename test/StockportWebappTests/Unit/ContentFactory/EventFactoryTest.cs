using System;
using System.Collections.Generic;
using Moq;
using StockportWebapp.ContentFactory;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using Xunit;
using FluentAssertions;

namespace StockportWebappTests.Unit.ContentFactory
{
    public class EventFactoryTest
    {
        private readonly EventFactory _factory;
        private readonly Mock<MarkdownWrapper> _markdownWrapper;
        private readonly Mock<ISimpleTagParserContainer> _tagParserContainer;
        private readonly Mock<IDynamicTagParser<Document>> _documentTagParser;
        private readonly Event _event;
        private const string Title = "This is the event";
        private const string Description = "The event";
        private const string Slug = "event-of-the-century";
        private const string Teaser = "Read more for the event";
        private const string Image = "image.jpg";
        private const string ThumbnailImage = "thumbnail.jpg";
        private const string Fee = "Free";
        private const string Location = "Bramall Hall, Carpark, SK7 6HG";
        private const string SubmittedBy = "Friends of Stockport";
        private const string StartTime = "10:00";
        private const string EndTime = "17:00";
        private readonly DateTime _eventDate = new DateTime(2016, 12, 30);
        private readonly List<Crumb> _breadcrumbs = new List<Crumb>();
        private readonly string _bookingInformation = "Booking information";
        private readonly List<Alert> _alerts = new List<Alert> { new Alert("title", "subHeading", "body", "severity", new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                                 new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty)};

        public EventFactoryTest()
        {
            _markdownWrapper = new Mock<MarkdownWrapper>();
            _tagParserContainer = new Mock<ISimpleTagParserContainer>();
            _documentTagParser = new Mock<IDynamicTagParser<Document>>();
            _factory = new EventFactory(_tagParserContainer.Object, _markdownWrapper.Object, _documentTagParser.Object);
            _event = new Event { Title = Title,  Slug = Slug,  Teaser = Teaser,  ImageUrl = Image,  ThumbnailImageUrl = ThumbnailImage, Description = Description, Fee = Fee, Location = Location,
                                 SubmittedBy = SubmittedBy, EventDate = _eventDate, StartTime = StartTime,
                                 EndTime = EndTime, Breadcrumbs = _breadcrumbs, BookingInformation = _bookingInformation, Alerts = _alerts};

            _tagParserContainer.Setup(o => o.ParseAll(Description, It.IsAny<string>())).Returns(Description);
            _markdownWrapper.Setup(o => o.ConvertToHtml(Description)).Returns(Description);
            _documentTagParser.Setup(o => o.Parse(Description, _event.Documents)).Returns(Description);
        }

        [Fact]
        public void ShouldSetTheCorrespondingFieldsForAProcessedEvent()
        {
            var result = _factory.Build(_event);
            result.Title.Should().Be("This is the event");
            result.Description.Should().Be("The event");
            result.Slug.Should().Be("event-of-the-century");
            result.Teaser.Should().Be("Read more for the event");
            result.Fee.Should().Be("Free");
            result.Location.Should().Be("Bramall Hall, Carpark, SK7 6HG");
            result.SubmittedBy.Should().Be("Friends of Stockport");
            result.EventDate.Should().Be(new DateTime(2016, 12, 30));
            result.StartTime.Should().Be("10:00");
            result.EndTime.Should().Be("17:00");
            result.BookingInformation.Should().Be("Booking information");
            result.Alerts[0].Title.Should().Be(_alerts[0].Title);
            result.Alerts[0].Body.Should().Be(_alerts[0].Body);
            result.Alerts[0].Severity.Should().Be(_alerts[0].Severity);
            result.Alerts[0].SubHeading.Should().Be(_alerts[0].SubHeading); 
            result.Alerts[0].SunriseDate.Should().Be(_alerts[0].SunriseDate);
            result.Alerts[0].SunsetDate.Should().Be(_alerts[0].SunsetDate);
        }

        [Fact]
        public void ShouldProcessDescriptionWithMarkdown()
        {
            _factory.Build(_event);

            _markdownWrapper.Verify(o => o.ConvertToHtml(Description), Times.Once);
        }   

        [Fact]
        public void ShouldPassTitleToAllSimpleParsersWhenBuilding()
        {
            _factory.Build(_event);

            _tagParserContainer.Verify(o => o.ParseAll(Description, _event.Title), Times.Once);
        }

        [Fact]
        public void ShouldProcessDescriptionWithDocumentParser()
        {
            _factory.Build(_event);

            _documentTagParser.Verify(o => o.Parse(Description, _event.Documents), Times.Once);
        }
    }
}
