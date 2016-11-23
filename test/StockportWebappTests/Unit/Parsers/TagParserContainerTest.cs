using System.Collections.Generic;
using FluentAssertions;
using Moq;
using StockportWebapp.Parsers;
using Xunit;

namespace StockportWebappTests.Unit.Parsers
{
    public class SimpleTagParserContainerTests
    {
        private readonly SimpleTagParserContainer _simpleTagParserCollection;
        private readonly Mock<ISimpleTagParser> _tagParser;
        private readonly Mock<ISimpleTagParser> _anotherTagParser;
        private readonly Mock<ISimpleTagParser> _anotherAnotherTagParser;

        public SimpleTagParserContainerTests()
        {
            _tagParser = new Mock<ISimpleTagParser>();
            _anotherTagParser = new Mock<ISimpleTagParser>();
            _anotherAnotherTagParser = new Mock<ISimpleTagParser>();
            var tagParserList = new List<ISimpleTagParser> { _tagParser.Object, _anotherTagParser.Object, _anotherAnotherTagParser.Object };

            _simpleTagParserCollection = new SimpleTagParserContainer(tagParserList);
        }

        [Fact]
        public void ShouldUseAllParsersToParseContent()
        {
            const string content = "some body";

            _tagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<string>())).Returns(content);
            _anotherTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<string>())).Returns(content);
            _anotherAnotherTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<string>())).Returns(content);

            _simpleTagParserCollection.ParseAll(content);

            _tagParser.Verify(o => o.Parse(content, It.IsAny<string>()), Times.Once);
            _anotherTagParser.Verify(o => o.Parse(content, It.IsAny<string>()), Times.Once);
            _anotherAnotherTagParser.Verify(o => o.Parse(content, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ShouldRemoveAllUnusedTags()
        {
            const string content = "{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}";

            _tagParser.Setup(o => o.Parse(content, It.IsAny<string>())).Returns(content);
            _anotherTagParser.Setup(o => o.Parse(content, It.IsAny<string>())).Returns(content);
            _anotherAnotherTagParser.Setup(o => o.Parse(content, It.IsAny<string>())).Returns(content);

            var parsedContent = _simpleTagParserCollection.ParseAll(content);

            parsedContent.Should().Be("CONTENT");
        }

        [Fact]
        public void ShouldPassTitleToParsers()
        {
            const string content = "Unimportant content";
            const string title = "The title";

            _tagParser.Setup(o => o.Parse(content, title)).Returns(content);
            _anotherTagParser.Setup(o => o.Parse(content, title)).Returns(content);
            _anotherAnotherTagParser.Setup(o => o.Parse(content, title)).Returns(content);

            var parsedContent = _simpleTagParserCollection.ParseAll(content, title);

            _tagParser.Verify();
            _anotherTagParser.Verify();
            _anotherAnotherTagParser.Verify();
        }
    }
}
