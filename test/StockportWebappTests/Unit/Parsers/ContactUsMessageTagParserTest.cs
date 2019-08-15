using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Parsers
{
    public class ContactUsMessageTagParserTest
    {
        private readonly ContactUsMessageTagParser _tagParser;
        private readonly Mock<IViewRender> _viewRenderer;
        private const string Message = "This is a message";
        private const string DefaultBody = "default body";
        private readonly string _bodyWithContactUsMessageTag = $"This is some content { ContactUsTagParser.ContactUsMessageTagRegex } <form><form>";
        private readonly Advertisement _advertisement = new Advertisement(string.Empty, string.Empty, string.Empty, DateTime.MinValue,
            DateTime.MinValue, false, string.Empty, string.Empty);

        public ContactUsMessageTagParserTest()
        {
            _viewRenderer = new Mock<IViewRender>();

            _viewRenderer.Setup(o => o.Render("ContactUsMessage", It.IsAny<string>())).Returns($"<p>{ Message }</p>");

            _tagParser = new ContactUsMessageTagParser(_viewRenderer.Object); 
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldNotAddAnyMessageIfNoMessageGiven(string message)
        {
            var slug = "this-is-a-slug";
            var section = ProcessedSectionWithDefaultSlugAndBody();
            var anotherSection = ProcessedSectionWithDefaultSlugAndBody(slug: slug, body: _bodyWithContactUsMessageTag);
            var processedArticle = new ProcessedArticle("title", "slug", DefaultBody, "teaser", "meta description", new List<ProcessedSection>() { section, anotherSection }, "icon", "backgroundImage", "image", new List<Crumb>(), new List<Alert>(), DefaultTopic(), new List<Alert>(), _advertisement, null);

            _tagParser.Parse(processedArticle, message, slug);

            processedArticle.Body.Should().Be(DefaultBody);
            section.Body.Should().Be(DefaultBody);
            anotherSection.Body.Should().Be(_bodyWithContactUsMessageTag);

            _viewRenderer.Verify(o => o.Render(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public void ShouldAddErrorMessageToArticleBodyWithFormTagInsideIfEmptySlugGiven()
        {
            var processedArticle = new ProcessedArticle("title", "slug", _bodyWithContactUsMessageTag, "teaser", "meta description", new List<ProcessedSection>(), "icon", "backgroundImage", "image", new List<Crumb>(), new List<Alert>(), DefaultTopic(), new List<Alert>(), _advertisement, null);

            _tagParser.Parse(processedArticle, Message, "");

            processedArticle.Body.Should().Be($"This is some content <p>{ Message }</p> <form><form>");
        }

        [Fact]
        public void ShouldAddErrorMessageToFirstSectionBodyWithFormTagInsideIfArticleDoesntHaveFormIfEmptySlugGiven()
        {
            var section = ProcessedSectionWithDefaultSlugAndBody(body: _bodyWithContactUsMessageTag);
            var processedArticle = new ProcessedArticle("title", "slug", DefaultBody, "teaser", "meta description", new List<ProcessedSection>() { section }, "icon", "backgroundImage", "image", new List<Crumb>(), new List<Alert>(), DefaultTopic(), new List<Alert>(), _advertisement, null);

            _tagParser.Parse(processedArticle, Message, "");

            processedArticle.Body.Should().Be(DefaultBody);
            section.Body.Should().Be($"This is some content <p>{ Message }</p> <form><form>");
        }

        [Fact]
        public void ShouldAddErrorMessageToSectionBodyWithFormTagInsideIfCorrespondingSlugGiven()
        {
            var slug = "this-is-a-slug";
            var section = ProcessedSectionWithDefaultSlugAndBody();
            var anotherSection = ProcessedSectionWithDefaultSlugAndBody(slug: slug, body: _bodyWithContactUsMessageTag);
            var processedArticle = new ProcessedArticle("title", "slug", DefaultBody, "teaser", "meta description", new List<ProcessedSection>() { section, anotherSection }, "icon", "backgroundImage", "image", new List<Crumb>(), new List<Alert>(), DefaultTopic(), new List<Alert>(), _advertisement, null);

            _tagParser.Parse(processedArticle, Message, slug);

            processedArticle.Body.Should().Be(DefaultBody);
            section.Body.Should().Be(DefaultBody);
            anotherSection.Body.Should().Be($"This is some content <p>{ Message }</p> <form><form>");
        }

        [Fact]
        public void ShouldDoNothingIfSlugProvidedButNoSectionsAreProvided()
        {
            var slug = "this-is-a-slug";
            var processedArticle = new ProcessedArticle("title", "slug", DefaultBody, "teaser", "meta description", new List<ProcessedSection>() { }, "icon", "backgroundImage", "image", new List<Crumb>(), new List<Alert>(), DefaultTopic(), new List<Alert>(), _advertisement, null);

            _tagParser.Parse(processedArticle, Message, slug);

            processedArticle.Body.Should().Be(DefaultBody);
        }

        [Fact]
        public void ShouldRenderMessage()
        {
            var slug = "this-is-a-slug";

            var advertisement = new Advertisement(string.Empty, string.Empty, string.Empty, DateTime.MinValue,
                DateTime.MinValue, false, string.Empty, string.Empty);

            var processedArticle = new ProcessedArticle("title", "slug", DefaultBody, "teaser", "meta description", new List<ProcessedSection>() { }, "icon", "backgroundImage", "image", new List<Crumb>(), new List<Alert>(), DefaultTopic(), new List<Alert>(), advertisement, null);

            _tagParser.Parse(processedArticle, Message, slug);
            
            _viewRenderer.Verify(o => o.Render("ContactUsMessage", Message), Times.Once);
        }

        private static ProcessedSection ProcessedSectionWithDefaultSlugAndBody(string slug = "slug", string body = DefaultBody)
        {
            return new ProcessedSection("title", slug, body, new List<Profile>(), new List<Document>(), new List<Alert>());
        }

        private static Topic DefaultTopic()
        {
            return new Topic("name", "slug", "summary", "teaser", "icon", "backgroundImage", "image", new List<SubItem>(), new List<SubItem>(), new List<SubItem>(), new List<Crumb>(), new List<Alert>(), true, "test-id", null, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, new Advertisement(string.Empty, string.Empty, string.Empty, DateTime.MinValue,DateTime.MinValue, false, string.Empty, string.Empty));
        }
    }
}
