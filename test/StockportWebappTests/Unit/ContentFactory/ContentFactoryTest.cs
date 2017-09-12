using System;
using System.Collections.Generic;
using StockportWebapp.Models;
using Xunit;
using Helper = StockportWebappTests.TestHelper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using StockportWebapp.ContentFactory;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebappTests.Unit.ContentFactory
{
    public class ContentFactoryTest
    {
        private readonly ContentTypeFactory _factory;

        public ContentFactoryTest()
        {
            var tagParserContainer = new Mock<ISimpleTagParserContainer>();
            var profileTagParser = new Mock<IDynamicTagParser<Profile>>();
            var documentTagParser = new Mock<IDynamicTagParser<Document>>();
            var alertsInlineTagParser = new Mock<IDynamicTagParser<Alert>>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            tagParserContainer.Setup(o => o.ParseAll(It.IsAny<string>(), It.IsAny<string>())).Returns("");

            _factory = new ContentTypeFactory(tagParserContainer.Object, profileTagParser.Object, new MarkdownWrapper(), documentTagParser.Object, alertsInlineTagParser.Object, httpContextAccessor.Object);
        }

        [Fact]
        public void ItUsesSectionFactoryToBuildProcessedSectionFromSection()
        {
            var section = new Section(Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<Profile>(), new List<Document>(), new List<Alert>());

            var processedSection = _factory.Build<Section>(section);

            processedSection.Should().BeOfType<ProcessedSection>();
        }

        [Fact]
        public void ItUsesProfileFactoryToBuildProcessedProfileFromProfile()
        {
            var profile = new Profile(Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<Crumb>());


            var processedProfile = _factory.Build<Profile>(profile);

            processedProfile.Should().BeOfType<ProcessedProfile>();
        }

        [Fact]
        public void ItUsesArticleFactoryToBuildProcessedArticleFromArticle()
        {
            var article = new Article(Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString,
                new List<Section>(), Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<Crumb>(), new List<Profile>(), new List<Document>(), false, new LiveChat(Helper.AnyString, Helper.AnyString), new List<Alert>());

            var processedArticle = _factory.Build<Article>(article);

            processedArticle.Should().BeOfType<ProcessedArticle>();
        }

        [Fact]
        public void ItUsesNewsFactoryToBuildProcessedNewsFromNews()
        {
            var news = new News(Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<Crumb>(), new DateTime(), new DateTime(), new List<Alert>(), new List<string>(), new List<Document>());

            var processed = _factory.Build<News>(news);

            processed.Should().BeOfType<ProcessedNews>();
        }
    }
}