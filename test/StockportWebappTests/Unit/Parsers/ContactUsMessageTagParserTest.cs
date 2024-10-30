using Amazon.SimpleEmail.Model;
using SharpKml.Dom;
using StockportWebapp.Models;
using static System.Collections.Specialized.BitVector32;
using System.Collections.Generic;
using Org.BouncyCastle.Bcpg.OpenPgp;
using StockportGovUK.NetStandard.Gateways.Models.RevsAndBens;

namespace StockportWebappTests_Unit.Unit.Parsers;

public class ContactUsMessageTagParserTest
{
    private readonly ContactUsMessageTagParser _tagParser;
    private readonly Mock<IViewRender> _viewRenderer;
    private const string Message = "This is a message";
    private const string DefaultBody = "default body";
    private const string MetaDescription = "default meta description";
    private readonly string _bodyWithContactUsMessageTag = $"This is some content {ContactUsTagParser.ContactUsMessageTagRegex} <form><form>";


    public ContactUsMessageTagParserTest()
    {
        _viewRenderer = new Mock<IViewRender>();

        _viewRenderer.Setup(o => o.Render("ContactUsMessage", It.IsAny<string>())).Returns($"<p>{Message}</p>");

        _tagParser = new ContactUsMessageTagParser(_viewRenderer.Object);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ShouldNotAddAnyMessageIfNoMessageGiven(string message)
    {
        string slug = "this-is-a-slug";
        ProcessedSection section = ProcessedSectionWithDefaultSlugAndBody();
        ProcessedSection anotherSection = ProcessedSectionWithDefaultSlugAndBody(slug: slug, body: _bodyWithContactUsMessageTag);
        ProcessedArticle processedArticle = new("title",
                                                "slug",
                                                DefaultBody,
                                                "teaser",
                                                "meta description",
                                                new List<ProcessedSection>() { section, anotherSection },
                                                "icon",
                                                "backgroundImage",
                                                "image",
                                                "alt",
                                                new List<Crumb>(),
                                                new List<Alert>(),
                                                DefaultTopic(),
                                                new List<Alert>(),
                                                new DateTime(),
                                                new bool(),
                                                null,
                                                "logo",
                                                null,
                                                string.Empty,
                                                string.Empty,
                                                new DateTime(),
                                                new List<InlineQuote>());
        _tagParser.Parse(processedArticle, message, slug);

        processedArticle.Body.Should().Be(DefaultBody);
        section.Body.Should().Be(DefaultBody);
        anotherSection.Body.Should().Be(_bodyWithContactUsMessageTag);

        _viewRenderer.Verify(o => o.Render(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public void ShouldAddErrorMessageToArticleBodyWithFormTagInsideIfEmptySlugGiven()
    {
        ProcessedArticle processedArticle = new("title",
                                                "slug",
                                                _bodyWithContactUsMessageTag,
                                                "teaser",
                                                "meta description",
                                                new List<ProcessedSection>(),
                                                "icon",
                                                "backgroundImage",
                                                "image",
                                                "altText",
                                                new List<Crumb>(),
                                                new List<Alert>(),
                                                DefaultTopic(),
                                                new List<Alert>(),
                                                new DateTime(),
                                                new bool(),
                                                null,
                                                "logo",
                                                null,
                                                string.Empty,
                                                string.Empty,
                                                new DateTime(),
                                                new List<InlineQuote>());
        _tagParser.Parse(processedArticle, Message, string.Empty);

        processedArticle.Body.Should().Be($"This is some content <p>{Message}</p> <form><form>");
    }

    [Fact]
    public void ShouldAddErrorMessageToFirstSectionBodyWithFormTagInsideIfArticleDoesntHaveFormIfEmptySlugGiven()
    {
        ProcessedSection section = ProcessedSectionWithDefaultSlugAndBody(body: _bodyWithContactUsMessageTag);
        ProcessedArticle processedArticle = new("title",
                                                "slug",
                                                DefaultBody,
                                                "teaser",
                                                "meta description",
                                                new List<ProcessedSection>() { section },
                                                "icon",
                                                "backgroundImage",
                                                "image",
                                                "alt",
                                                new List<Crumb>(),
                                                new List<Alert>(),
                                                DefaultTopic(),
                                                new List<Alert>(),
                                                new DateTime(),
                                                new bool(),
                                                null,
                                                "logo",
                                                null,
                                                string.Empty,
                                                string.Empty,
                                                new DateTime(),
                                                new List<InlineQuote>());

        _tagParser.Parse(processedArticle, Message, string.Empty);

        processedArticle.Body.Should().Be(DefaultBody);
        section.Body.Should().Be($"This is some content <p>{Message}</p> <form><form>");
    }

    [Fact]
    public void ShouldAddErrorMessageToSectionBodyWithFormTagInsideIfCorrespondingSlugGiven()
    {
        string slug = "this-is-a-slug";
        ProcessedSection section = ProcessedSectionWithDefaultSlugAndBody();
        ProcessedSection anotherSection = ProcessedSectionWithDefaultSlugAndBody(slug: slug, body: _bodyWithContactUsMessageTag);
        ProcessedArticle processedArticle = new("title",
                                                "slug",
                                                DefaultBody,
                                                "teaser",
                                                "meta description",
                                                new List<ProcessedSection>() { section, anotherSection },
                                                "icon",
                                                "backgroundImage",
                                                "image",
                                                "alt",
                                                new List<Crumb>(),
                                                new List<Alert>(),
                                                DefaultTopic(),
                                                new List<Alert>(),
                                                new DateTime(),
                                                new bool(),
                                                null,
                                                "logo",
                                                null,
                                                string.Empty,
                                                string.Empty,
                                                new DateTime(),
                                                new List<InlineQuote>());

        _tagParser.Parse(processedArticle, Message, slug);

        processedArticle.Body.Should().Be(DefaultBody);
        section.Body.Should().Be(DefaultBody);
        anotherSection.Body.Should().Be($"This is some content <p>{Message}</p> <form><form>");
    }

    [Fact]
    public void ShouldDoNothingIfSlugProvidedButNoSectionsAreProvided()
    {
        string slug = "this-is-a-slug";
        ProcessedArticle processedArticle = new("title",
                                                "slug",
                                                DefaultBody,
                                                "teaser",
                                                "meta description",
                                                new List<ProcessedSection>() { },
                                                "icon",
                                                "backgroundImage",
                                                "image",
                                                "alt",
                                                new List<Crumb>(),
                                                new List<Alert>(),
                                                DefaultTopic(),
                                                new List<Alert>(),
                                                new DateTime(),
                                                new bool(),
                                                null,
                                                "logo",
                                                null,
                                                string.Empty,
                                                string.Empty,
                                                new DateTime(),
                                                new List<InlineQuote>());

        _tagParser.Parse(processedArticle, Message, slug);

        processedArticle.Body.Should().Be(DefaultBody);
    }

    [Fact]
    public void ShouldRenderMessage()
    {
        string slug = "this-is-a-slug";
        ProcessedArticle processedArticle = new ("title",
                                                "slug",
                                                DefaultBody,
                                                "teaser",
                                                "meta description",
                                                new List<ProcessedSection>() { },
                                                "icon",
                                                "backgroundImage",
                                                "image",
                                                "alt",
                                                new List<Crumb>(),
                                                new List<Alert>(),
                                                DefaultTopic(),
                                                new List<Alert>(),
                                                new DateTime(),
                                                new bool(),
                                                null,
                                                "logo",
                                                null,
                                                string.Empty,
                                                string.Empty,
                                                new DateTime(),
                                                new List<InlineQuote>());

        _tagParser.Parse(processedArticle, Message, slug);

        _viewRenderer.Verify(o => o.Render("ContactUsMessage", Message), Times.Once);
    }

    private static ProcessedSection ProcessedSectionWithDefaultSlugAndBody(string slug = "slug", string body = DefaultBody, string metaDescription = MetaDescription) 
        => new("title",
                slug,
                metaDescription,
                body,
                new List<Profile>(),
                null,
                new List<Alert>(),
                new List<GroupBranding>(),
                "logoAreaTitle",
                new DateTime());

    private static Topic DefaultTopic()
        => new("name",
               "slug",
               "summary",
               "teaser",
               "metaDescription",
               "icon",
               "backgroundImage",
               "image",
               new List<SubItem>(),
               new List<SubItem>(),
               new List<SubItem>(),
               new List<Crumb>(),
               new List<Alert>(),
               true,
               "test-id",
               null,
               string.Empty,
               true,
               new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty),
               string.Empty,
               null,
               string.Empty);
}
