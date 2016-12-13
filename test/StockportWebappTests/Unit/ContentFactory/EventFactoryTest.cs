using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly DateTime _sunriseDate = new DateTime(2016, 08, 01);
        private readonly DateTime _sunsetDate = new DateTime(2016, 08, 10);
        private const string Image = "image.jpg";
        private const string ThumbnailImage = "thumbnail.jpg";
        private const string Fee = "Free";
        private const string Location = "Bramall Hall, Carpark, SK7 6HG";
        private const string SubmittedBy = "Friends of Stockport";
        private const string StartTime = "10:00";
        private const string EndTime = "17:00";
        private readonly DateTime _eventDate = new DateTime(2016, 12, 30);

        public EventFactoryTest()
        {
            _markdownWrapper = new Mock<MarkdownWrapper>();
            _tagParserContainer = new Mock<ISimpleTagParserContainer>();
            _documentTagParser = new Mock<IDynamicTagParser<Document>>();
            _factory = new EventFactory(_tagParserContainer.Object, _markdownWrapper.Object, _documentTagParser.Object);
            _event = new Event( Title,  Slug,  Teaser,  Image,  ThumbnailImage,  Description,  Fee,  Location,
             SubmittedBy,  string.Empty,  string.Empty, false, _eventDate,  StartTime,  EndTime);

            _tagParserContainer.Setup(o => o.ParseAll(Description, It.IsAny<string>())).Returns(Description);
            _markdownWrapper.Setup(o => o.ConvertToHtml(Description)).Returns(Description);
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
            result.Featured.Should().Be(false);
            result.EventDate.Should().Be(new DateTime(2016, 12, 30));
            result.StartTime.Should().Be("10:00");
            result.EndTime.Should().Be("17:00");
        }

        [Fact]
        public void ShouldProcessDescriptionWithMarkdown()
        {
            _factory.Build(_event);

            _markdownWrapper.Verify(o => o.ConvertToHtml(Description), Times.Once);
        }   

        [Fact]
        public void ShouldPassTitleToParserWhenBuilding()
        {
            _factory.Build(_event);

            _tagParserContainer.Verify(o => o.ParseAll(Description, _event.Title), Times.Once);
        }
    }
}
